using iBoxDB.LocalServer;
using Net.Models;
using System;
using System.Collections.Generic;

namespace Net.Api
{
    public class Engine
    {
        private readonly Util util = new Util();
        private readonly StringUtil sUtil = new StringUtil();
        public long maxSearchTime = long.MaxValue;

        public void Config(DatabaseConfig config)
        {
            KeyWord.config(config);
        }

        public long indexText(IBox box, long id, String str, bool isRemove)
        {
            if (id == -1)
            {
                return -1;
            }
            long itCount = 0;
            char[] cs = sUtil.clear(str);
            List<KeyWord> map = util.fromString(id, cs, true);

            HashSet<String> words = new HashSet<String>();
            foreach (KeyWord kw in map)
            {
                insertToBox(box, kw, words, isRemove);
                itCount++;
            }
            return itCount;
        }

        public long indexTextNoTran(AutoBox auto, int commitCount, long id, String str, bool isRemove)
        {
            if (id == -1)
            {
                return -1;
            }
            long itCount = 0;
            char[] cs = sUtil.clear(str);
            List<KeyWord> map = util.fromString(id, cs, true);

            HashSet<String> words = new HashSet<String>();
            IBox box = null;
            int ccount = 0;
            foreach (KeyWord kw in map)
            {
                if (box == null)
                {
                    box = auto.Cube();
                    ccount = commitCount;
                }
                insertToBox(box, kw, words, isRemove);
                itCount++;
                if (--ccount < 1)
                {
                    box.Commit().Assert();
                    box = null;
                }
            }
            if (box != null)
            {
                bool issucceed = box.Commit().Assert();
            }
            return itCount;
        }

        private static void insertToBox(IBox box, KeyWord kw, HashSet<String> insertedWords, bool isRemove)
        {
            Binder binder;
            if (kw is KeyWordE)
            {
                if (insertedWords.Contains(kw.KWord.ToString()))
                {
                    return;
                }
                insertedWords.Add(kw.KWord.ToString());
                binder = box["/E", kw.KWord, kw.ID, kw.Position];
            }
            else
            {
                binder = box["/N", kw.KWord, kw.ID, kw.Position];
            }
            if (isRemove)
            {
                binder.Delete();
            }
            else
            {
                if (binder.TableName == "/E")
                {
                    binder.Insert((KeyWordE)kw);
                }
                else
                {
                    binder.Insert((KeyWordN)kw);
                }
            }
        }

        public SortedSet<String> discover(IBox box,
                                            char efrom, char eto, int elength,
                                            char nfrom, char nto, int nlength)
        {
            SortedSet<String> list = new SortedSet<String>();
            Random ran = new Random();
            if (elength > 0)
            {
                int len = ran.Next(KeyWord.MAX_WORD_LENGTH) + 1;
                char[] cs = new char[len];
                for (int i = 0; i < cs.Length; i++)
                {
                    cs[i] = (char)(ran.Next(eto - efrom) + efrom);
                }
                KeyWordE kw = new KeyWordE();
                kw.KWord = new String(cs);
                foreach (KeyWord tkw in lessMatch(box, kw))
                {
                    String str = tkw.KWord.ToString();
                    if (str.Length < 3 || sUtil.mvends.Contains(str))
                    {
                        continue;
                    }
                    int c = list.Count;
                    list.Add(str);
                    if (list.Count > c)
                    {
                        elength--;
                        if (elength <= 0)
                        {
                            break;
                        }
                    }
                }
            }
            if (nlength > 0)
            {
                char[] cs = new char[2];
                for (int i = 0; i < cs.Length; i++)
                {
                    cs[i] = (char)(ran.Next(nto - nfrom) + nfrom);
                }
                KeyWordN kw = new KeyWordN();
                kw.longKeyWord(cs[0], cs[1], (char)0);
                foreach (KeyWord tkw in lessMatch(box, kw))
                {
                    int c = list.Count;
                    list.Add(((KeyWordN)tkw).toKString());
                    if (list.Count > c)
                    {
                        nlength--;
                        if (nlength <= 0)
                        {
                            break;
                        }
                    }
                }
            }
            return list;
        }

        public IEnumerable<KeyWord> SearchDistinct(IBox box, String str)
        {
            //todo add serarch words
            long c_id = -1;
            foreach (KeyWord kw in Search(box, str))
            {
                if (kw.ID == c_id)
                {
                    continue;
                }
                c_id = kw.ID;
                yield return kw;
            }
        }

        public String getDesc(String str, KeyWord kw, int length)
        {
            return sUtil.getDesc(str, kw, length);
        }

        public IEnumerable<KeyWord> Search(IBox box, String str)
        {
            char[] cs = sUtil.clear(str);
            List<KeyWord> map = util.fromString(-1, cs, false);
            sUtil.correctInput(map);

            if (map.Count > KeyWord.MAX_WORD_LENGTH || map.Count == 0)
            {
                return new List<KeyWord>();
            }

            List<KeyWord> kws = new List<KeyWord>();

            for (int i = 0; i < map.Count; i++)
            {
                KeyWord kw = map[i];
                if (kw is KeyWordE)
                {
                    String s = kw.KWord.ToString();
                    if ((s.Length > 2) && (!sUtil.mvends.Contains(s)))
                    {
                        kws.Add(kw);
                        map[i] = null;
                    }
                }
                else
                {
                    KeyWordN kwn = (KeyWordN)kw;
                    if (kwn.size() >= 2)
                    {
                        kws.Add(kw);
                        map[i] = null;
                    }
                    else if (kws.Count > 0)
                    {
                        KeyWord p = kws[kws.Count - 1];
                        if (p is KeyWordN)
                        {
                            if (kwn.Position == (p.Position + ((KeyWordN)p).size()))
                            {
                                kws.Add(kw);
                                map[i] = null;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < map.Count; i++)
            {
                KeyWord kw = map[i];
                if (kw != null)
                {
                    kws.Add(kw);
                }
            }
            MaxID maxId = new MaxID(this.maxSearchTime);
            return Search(box, kws.ToArray(), maxId);
        }

        private IEnumerable<KeyWord> Search(IBox box, KeyWord[] kws, MaxID maxId)
        {
            if (kws.Length == 1)
            {
                return Search(box, kws[0], (KeyWord)null, false, maxId);
            }
            bool asWord = true;
            KeyWord kwa = kws[kws.Length - 2];
            KeyWord kwb = kws[kws.Length - 1];
            if ((kwa is KeyWordN) && (kwb is KeyWordN))
            {
                asWord = kwb.Position != (kwa.Position + ((KeyWordN)kwa).size());
            }

            KeyWord[] condition = new KeyWord[kws.Length - 1];
            Array.Copy(kws, 0, condition, 0, condition.Length);
            return Search(box, kws[kws.Length - 1], Search(box, condition, maxId), asWord, maxId);
        }

        private IEnumerable<KeyWord> Search(IBox box, KeyWord nw,
                                             IEnumerable<KeyWord> condition, bool isWord, MaxID maxId)
        {
            long r1_id = -1;
            foreach (KeyWord r1_con in condition)
            {
                if (isWord)
                {
                    if (r1_id == r1_con.ID)
                    {
                        continue;
                    }
                }
                r1_id = r1_con.ID;
                foreach (KeyWord k in Search(box, nw, r1_con, isWord, maxId))
                {
                    k.previous = r1_con;
                    yield return k;
                }
            }
        }

        private static readonly List<KeyWord> emptySearch = new List<KeyWord>();

        private static IEnumerable<KeyWord> Search(IBox box, KeyWord kw, KeyWord con, bool asWord, MaxID maxId)
        {
            if (kw is KeyWordE)
            {
                asWord = true;
                return new Index2KeyWordIterable<KeyWordE>(
                    box.Select<Object>("from /E where K==? & I<=?",
                                     kw.KWord, maxId.id), box, kw, con, asWord, maxId);
            }
            else
            {
                if (con is KeyWordE)
                {
                    asWord = true;
                }
                if (con == null || asWord)
                {
                    asWord = true;
                    return new Index2KeyWordIterable<KeyWordN>(
                        box.Select<Object>("from /N where K==? & I<=?", kw.KWord, maxId.id), box, kw, con, asWord, maxId);
                }
                else
                {
                    Object[] os = (Object[])box["/N", kw.KWord,
                                                 con.ID, (con.Position + ((KeyWordN)con).size())]
                        .Select<Object>();
                    if (os != null)
                    {
                        KeyWordN cache = new KeyWordN();
                        cache.KWord = os[0];
                        cache.I = (long)os[1];
                        cache.P = (int)os[2];
                        List<KeyWord> r = new List<KeyWord>(1);
                        r.Add(cache);
                        return r;
                    }
                    else
                    {
                        return emptySearch;
                    }
                }
            }
        }

        private static IEnumerable<KeyWord> lessMatch(IBox box, KeyWord kw)
        {
            if (kw is KeyWordE)
            {
                return new Index2KeyWordIterable<KeyWordE>(box.Select<object>("from /E where K<=? limit 0, 50", kw.KWord), null, null, null, true, new MaxID(long.MaxValue));
            }
            else
            {
                return new Index2KeyWordIterable<KeyWordN>(box.Select<object>("from /N where K<=? limit 0, 50", kw.KWord), null, null, null, true, new MaxID(long.MaxValue));
            }
        }

        private sealed class MaxID
        {
            public MaxID(long maxSearchTime)
            {
                maxTime = maxSearchTime;
            }

            public long maxTime;
            public long id = long.MaxValue;
        }

        private interface IIndex2KeyWordIterable
        {
            IEnumerator<Object> GetIndex();
        }

        private class Index2KeyWordIterable<T> : IIndex2KeyWordIterable, IEnumerable<KeyWord> where T : KeyWord, new()
        {
            private readonly KWIterator iterator;
            private IEnumerator<Object> index;

            internal Index2KeyWordIterable(IEnumerable<Object> findex,
                                            IBox box, KeyWord kw, KeyWord con, bool asWord, MaxID maxId)
            {
                this.index = findex.GetEnumerator();
                this.iterator = new KWIterator();

                long currentMaxId = maxId.id;
                KeyWord cache = null;
                this.iterator.moveNext = () =>
                {
                    if (maxId.id == -1)
                    {
                        return false;
                    }
                    if (con != null)
                    {
                        if (con.I != maxId.id)
                        {
                            return false;
                        }
                    }
                    if (currentMaxId > (maxId.id + 1) && currentMaxId != long.MaxValue)
                    {
                        currentMaxId = maxId.id;

                        IEnumerable<KeyWord> tmp = Search(box, kw, con, asWord, maxId);
                        if (tmp is IIndex2KeyWordIterable)
                        {
                            index = ((IIndex2KeyWordIterable)tmp).GetIndex();
                        }
                    }
                    if (index.MoveNext())
                    {
                        if (--maxId.maxTime < 0)
                        {
                            maxId.id = -1;
                            return false;
                        }

                        Object[] os = (Object[])index.Current;

                        long osid = (long)os[1];
                        maxId.id = osid;
                        currentMaxId = maxId.id;

                        if (con != null)
                        {
                            if (con.I != maxId.id)
                            {
                                return false;
                            }
                        }

                        cache = typeof(T) == typeof(KeyWordE) ? (KeyWord)new KeyWordE() : new KeyWordN();
                        cache.KWord = os[0];
                        cache.I = (long)os[1];
                        cache.P = (int)os[2];

                        return true;
                    }
                    maxId.id = -1;
                    return false;
                };

                this.iterator.current = () =>
                {
                    return cache;
                };
            }

            internal class KWIterator : IEnumerator<KeyWord>
            {
                public delegate bool MoveNextDelegate();

                public delegate KeyWord CurrentDelegate();

                public MoveNextDelegate moveNext;
                public CurrentDelegate current;

                public bool MoveNext()
                {
                    return moveNext();
                }

                public KeyWord Current
                {
                    get
                    {
                        return current();
                    }
                }

                #region IEnumerator implementation

                void System.Collections.IEnumerator.Reset()
                {
                }

                object System.Collections.IEnumerator.Current
                {
                    get
                    {
                        return this.Current;
                    }
                }

                void IDisposable.Dispose()
                {
                }

                #endregion IEnumerator implementation
            }

            public IEnumerator<KeyWord> GetEnumerator()
            {
                return iterator;
            }

            #region IEnumerable implementation

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion IEnumerable implementation

            #region IIndex2KeyWordIterable implementation

            public IEnumerator<object> GetIndex()
            {
                return index;
            }

            #endregion IIndex2KeyWordIterable implementation
        }
    }
}