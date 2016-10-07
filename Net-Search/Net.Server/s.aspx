<%@ Page Language="C#" Inherits="Net.Server.Default" EnableSessionState="false" Async="true" AsyncTimeout="30" %>

<%@ Import Namespace="Net.Server" %>
<%@ Import Namespace="Net.Api" %>
<%@ Import Namespace="Net.Models" %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="content-type" content="text/html; charset=utf-8">
    <meta charset="utf-8">
    <meta name="description" content="<%=name%>">
    <title><%=name%>,</title>

    <link rel="stylesheet" type="text/css" href="content/semantic.min.css">

    <style>
        body {
            margin-top: 10px;
            margin-left: 10px;
            font-weight: lighter;
            overflow-x: hidden;
        }

        #s_head {
            min-width: 1000px;
            border: red solid 0;
            height: 60px;
            position: relative;
            margin-left: -10px;
            margin-top: 0;
            overflow: visible;
        }

        #s_body {
            overflow: auto;
            padding-left: 30px;
            margin-left: 0;
            min-width: 1000px;
            border: red solid 0px;
        }

        #float_banner {
            top: 0;
            padding-left: 40px;
            border: red solid 0px;
            position: fixed;
            z-index: 3000;
        }

        #body_left {
            max-width: 600px;
        }

        #body_right {
            margin-left: 40px;
            min-width: 360px;
            max-width: 400px;
            border-left: 1px solid #eee;
            padding: 0 0 0 15px;
        }

        #right_show {
            -width: 360px;
        }

        #so_feb {
            width: 340px;
        }

        #soSafe {     
            border-top: 2px solid #aef0c6;
            border-bottom: 2px solid #aef0c6;     
            font-size: 13px;
            margin-bottom: 15px;
        }

            #soSafe dt {    
                  
                cursor: pointer;
                height: 22px;
                line-height: 22px;
            }

                #soSafe dt span {
                    color: #46c878;
                    font: 13px/22px "Microsoft YaHei";
                    vertical-align: middle;
                }


                #soSafe dt div {
                    background-position: -177px -366px;
                    color: #999;
                    float: right;
                    font-size: 12px;
                    padding-right: 14px;
                }

                #soSafe:hover {
                    border-color: #cef0c6;
                }

            /*#soSafe.open dt.hover {
                border-color: #fff;
            }*/

            #soSafe.open dt div {
                background-position: -177px -387px;
            }

            #soSafe.open dd {
                display: block;
            }

            #soSafe span.safe-guard-logo {
                background: url(src/safe_16.png) no-repeat;
                background-image: -webkit-image-set(url(src/safe_16.png) 1x,url(src/safe_32.png) 2x);
                display: inline-block;
                height: 13px;
                margin-right: 3px;
                vertical-align: -2px;
                width: 13px;
            }

            #soSafe dd {
                display: none;
                padding: 11px 0 12px;
            }

                #soSafe dd p {
                    color: #333;
                    line-height: 20px;
                    margin-bottom: 3px;
                }

                    #soSafe dd p.txt {
                        margin-bottom: 8px;
                    }

            .toask .ico-ask, .so-doc, .m-icp .icon-to, .result .res-list p.erro, #soSafe dt div {
                background-image: url(https://p.ssl.qhimg.com/t019016be2616bba0f8.png);
                background-repeat: no-repeat;
            }

        .stext {
            font-size: 13px;
            line-height: 20px;
        }

        .rt {
            color: red;
        }

        .res-title {
            margin-bottom: 0;
            margin-top: 0.2em;
        }

        .res-linkinfo cite {
            color: #4e9c62;
            font-size: 12px;
            font-style: normal;
        }

        .res-linkinfo a {
            font-style: normal;
            font-size: 12px;
            line-height: 20px;
        }

        .mingpian {
            color: #666;
            margin-left: -2px;
            text-align: center;
            font-size: 20px;
        }

            .mingpian:visited {
                color: #666;
            }

            .mingpian:hover {
                color: #00bb3c;
            }
            .mingpian span {
               font-size:12px;
            }

        .tip-v {
            background: url(src/v_16.png) no-repeat 0 0;
            background-image: -webkit-image-set(url(src/v_16.png) 1x,url(src/v_32.png) 2x);
            display: inline-block;
            height: 12px;
            overflow: hidden;
            vertical-align: middle;
            width: 12px;
        }

        #s_footer {
            clear: both;
            background: #FAFAFA;
            font-size: 12px;
            height: 60px;
            line-height: 53px;
            margin-top: 50px;
            background-color: #e8e8e8;
            border-color: #e8e8e8;
            background-image: none;
            padding: .6em .8em;
            color: #999;
            text-transform: none;
            margin-bottom: 0;
            position: relative;
            margin-left: -10px;
            padding-left: 40px;
        }

            #s_footer a {
                color: #999;
                font-size: 12px;
                margin: 0 8px;
                text-decoration: none;
            }

        .flex-container {
            padding: 0;
            margin: 0;
            list-style: none;
            display: -webkit-box;
            display: -moz-box;
            display: -ms-flexbox;
            display: -webkit-flex;
            display: flex;
            -webkit-flex-flow: row wrap;
            border: red solid 0;
        }

        .flex-item {
            background: tomato;
            padding: 5px;
            height: 150px;
            margin-top: 10px;
            line-height: 150px;
            color: white;
            font-weight: bold;
            font-size: 3em;
            text-align: center;
        }

        .spread li, .spread h3 {
            list-style: none;
            margin: 0;
            padding: 0;
        }

        #page {
            margin: 15px 0 0 -2px;
            padding-top: 6px;
            white-space: nowrap;
        }

            #page a, #page strong {
                background: white;
                border: 1px solid #e5e5e5;
                border-radius: 2px;
                display: inline-block;
                font-size: 16px;
                height: 36px;
                line-height: 36px;
                margin-right: 5px;
                text-align: center;
                text-decoration: none;
                vertical-align: middle;
                width: 36px;
            }

                #page a#spre, #page a#snext {
                    font-size: 14px;
                    width: 76px;
                }

            #page .nums {
                color: #999;
                font-size: 12px;
            }
    </style>

    <style type="text/css">
        * {
            margin: 0;
            padding: 0;
        }

        .nav-wrapper-fixed {
            position: fixed;
            top: 0;
            width: 100%;
        }

            .nav-wrapper-fixed .nav {
                width: 960px;
                margin: 0 auto;
            }

                .nav-wrapper-fixed .nav li {
                    float: left;
                    width: 100px;
                    margin-right: 5px;
                    background: #CCC;
                    text-align: center;
                    height: 24px;
                    line-height: 24px;
                    list-style: none;
                }

        .nav-wrapper {
            margin-top: 100px;
            width: 100%;
        }

            .nav-wrapper .nav {
                width: 960px;
                margin: 0 auto;
            }

                .nav-wrapper .nav li {
                    float: left;
                    width: 100px;
                    margin-right: 5px;
                    background: #CCC;
                    text-align: center;
                    height: 24px;
                    line-height: 24px;
                    list-style: none;
                }
    </style>

    <script>
        function highlight() {
            var txt = document.title.substr(0, document.title.indexOf(','));

            var ts = document.getElementsByClassName("stext");

            var kws = txt.split(/[ 　]/);
            for (var i = 0; i < kws.length; i++) {
                var kw = String(kws[i]).trim();
                if (kw.length < 1) {
                    continue;
                }
                var fontText = "<font class='rt'>";
                if (fontText.indexOf(kw.toLowerCase()) > -1) {
                    continue;
                }
                if ("</font>".indexOf(kw.toLowerCase()) > -1) {
                    continue;
                }
                for (var j = 0; j < ts.length; j++) {
                    var html = ts[j].innerHTML;
                    ts[j].innerHTML =
                            html.replace(new RegExp(kw, 'gi'),
                                    fontText + kw + "</font>");
                }
            }
        }
        </script>

    <script type="text/javascript">
        window.onload = function () {
            var nav = document.getElementById('nav');
            var navFixed = document.getElementById('navFixed');
            window.onscroll = function () {
                var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
                document.title = "<%=name%>"
                if (scrollTop > nav.offsetTop) {
                    navFixed.style.display = 'block';
                } else if (scrollTop < nav.offsetTop) {
                    navFixed.style.display = 'none';
                }
            }
        };
    </script>

      <script>
          var objA = null, intrval = null;
          var div_tip_ID = null;
          function show(obj) {
              if (!obj) {
                  obj = objA;
              }
              else objA = obj;

              if (intrval) {
                  window.clearTimeout(intrval);
                  intrval = null;
              }
              div_tip_ID = objA.id;

              var div_tip = document.getElementById(div_tip_ID);
              div_tip.style.display = "block";
              div_tip.style.left = (obj.offsetLeft + 20) + "px";
              div_tip.style.top = (obj.offsetTop - div_tip.offsetHeight) + "px";
          }
          function hide() {
              //现在这个demo提示框和超链接没重叠部分，所以延时50毫秒隐藏提示框,以解决移出超链接到移入提示框这个过程之间提示框隐藏掉。
              //大部分时候可能是做成有重叠的，就不需要延时隐藏。
              intrval = window.setTimeout(function () {
                  document.getElementById(div_tip_ID).style.display = "none";
              }, 50);

          }
  </script>
 
</head>
<body onload="highlight()">

    <div class="nav-wrapper-fixed" id="navFixed" style="display: none;">
        <%--   <div class="nav" id="nav">
            <ul>
                <li><a target="_blank" href="http://www.CsrCode.cn/">网页特效</a></li>
                <li><a target="_blank" href="http://www.7caidy.com/">七彩影视</a></li>
                <li><a target="_blank" href="http://www.33567.cn/">珊珊影视</a></li>
                <li><a target="_blank" href="http://Changshi.CsrCode.Cn/">生活常识</a></li>
                <li><a target="_blank" href="http://bbs.33567.cn/">外链论坛</a></li>
                <li><a target="_blank" href="http://dir.33567.cn/">分类目录</a></li>
            </ul>
        </div>--%>
    </div>
    <%--search box--%>

    <div id="s_head" class="column">
        <form class="ui large form" action="s.aspx" onsubmit="formsubmit()">
            <div class="ui label input" id="float_banner">
                <div class="ui action input" style="width: 580px">
                    <a href="./"><i class="teal feed icon" style="font-size: 42px"></i></a>
                    <input name="q" value="<%=name%>" required onfocus="formfocus()" />
                    <input id="btnsearch" type="submit" class="ui teal right button" value="Search" />
                </div>
            </div>
        </form>
        <script>
            function formsubmit() {
                btnsearch.disabled = "disabled";
            }
            function formfocus() {
                btnsearch.disabled = undefined;
            }
           </script>
    </div>

    <%--result box after searched--%>
    <div id="s_body">
        <div class="flex-container">
            <%--left side start--%>
            <div id="body_left">
                <% foreach (var p in pagesAll)
                   {
                       String content = null;
                       if (pagesAll.Count() == 1 || p.keyWord == null)
                       {
                           content = p.Description + "...";
                           if (p.Content != null)
                           {
                               content += p.Content.ToString();
                           }
                       }
                       else if (p.Id != p.keyWord.ID)
                       {
                           content = p.Description;
                           if (content.Length < 20)
                           {
                               content += p.GetRandomContent();
                           }
                       }
                       else
                       {
                           var c1 = p.Content != null ? p.Content.ToString() : p.Description;
                           content = SearchResource.Engine.getDesc(c1, p.keyWord, 80);
                           if (content.Length < 100)
                           {
                               content += p.GetRandomContent();
                           }
                           if (content.Length < 100)
                           {
                               content += p.Description;
                           }
                           if (content.Length > 200)
                           {
                               content = content.Substring(0, 200) + "..";
                           }
                       }
                        %>
                <h3 class="res-title">
                    <a class="stext" target="_blank" href="<%=p.Url%>"><%= p.Title%></a>
                </h3>
                <span class="stext"><%=content%> </span>
                <br />
                  <% if(p.Tag!=null)%>
                    <% 
                    {
                    %>
                    <div id="<%=p.Id%>" style='width:300px;height:200px;display:none;background:#aef0c6;position:absolute;' onmouseover="show()" onmouseout="this.style.display='none';">
                        <div class=\"m-title\"><%=((SiteInfo)p.Tag).Name%></div><div class=\"m-1\"><%=((SiteInfo)p.Tag).Description %>></div><div  class=\"m-2\">网站信用:</div><div  class=\"m-3\"><%=((SiteInfo)p.Tag).IsGuard %>加入上网保障计划</div>  
                    </div>
                    <% 
                    }
                    %>
                <div class="res-linkinfo" >
                    <cite><%=p.Url%> </cite>&nbsp;&nbsp;<%=p.Verified%>
                </div>
                <% 
                   }
                %>
            </div>
            <%--left side end--%>

            <%--right side start--%>
            <div id="body_right">
                <%-- <div class="ui segment">
                    <h4><a href="http://www.iboxdb.com" target="_blank">iBoxDB</a></h4>
                    Fast NoSQL Document Database
               
                </div>--%>

                <div id="so_feb">
                    <dl id="soSafe" class="open">
                        <dt class="" onclick='collapsed()'>
                            <div id="collapse-button">收起</div>
                            <span class="safe-guard-logo"></span><span>Net Search · 安全保障</span></dt>
                        <dd id="ad-home" style="display: none;">
                            <p class="txt">如您加入Net Search推广赔付计划，在Net Search推广网站中因遭遇欺诈、钓鱼、假冒网站并造成经济损失，在符合《Net Search推广赔付协议》的赔付条件时，可向Net Search申请赔付。<a href="#src=#" target="_blank" class="join">我要加入&gt;&gt;</a></p>
                            <p><strong>了解详情：</strong>        <a href="#src=#" target="_blank">网购先赔</a></p>
                            <p><strong>举报入口：</strong> <a href="https://110.xxx.cn" target="_blank">猎网平台&gt;&gt;</a></p>
                        </dd>
                    </dl>
                </div>
                <div id="right_show" class="spread">
                    <ul id="rightbox" class="result">
                        <li>
                            <h3><a href="" e_nolog="0" target="_blank" style="font-size: 15px" se_prerender_url="complete">想在Net Search推广您的产品服务吗？</a></h3>
                            <p class="res-linkinfo">
                                ☎ 4000-123-123<br>
                                <cite>a.bcd.cn</cite>
                            </p>
                        </li>
                    </ul>
                </div>
                <script>
                    function collapsed() {

                        if (document.getElementById('collapse-button').textContent == '收起') {
                            document.getElementById('collapse-button').textContent = '展开';
                            document.getElementById('soSafe').className = '';
                        }
                        else {
                            document.getElementById('collapse-button').textContent = '收起'
                            document.getElementById('soSafe').className = 'open';
                        }
                        var showorhidden = document.getElementById("ad-home");

                        if (showorhidden.style.display == 'none') {
                            showorhidden.style.display = 'block';
                        }
                        else { showorhidden.style.display = 'none' }
                    }
                </script>

                <%
                    String tcontent = (DateTime.Now - begin).TotalSeconds + "s, "
                            + "MEM:" + (System.GC.GetTotalMemory(false) / 1024 / 1024) + "MB ";
                %>
                <%--   <div class="ui segment">
                    Time: 
                    <%= tcontent%> |
           
                    Collected links: 
                    <%= processLinksCount%><br/>
               
                    Verified site: 
                    <%= siteInfoCount%> |
                
                    Collected pages: 
                    <%= sitePageCount%>
                </div>--%>
            </div>
            <%--right side end--%>
        </div>
        <div id="page">
            <span><%=pageIndexString%>  </span>
            <span class="nums" style="margin-left: 20px">找到相关结果约<%=searchResult %>个</span>
        </div>
    </div>

    <div id="s_footer">
        <a href="u" target="_blank">意见反馈</a> | <a href="" target="_blank">推广合作</a> | <span>Copyright © Net Search</span>
    </div>
</body>
</html>
