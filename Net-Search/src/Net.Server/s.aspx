﻿<%@ Page Language="C#" Inherits="Net.Server.Default" EnableSessionState="false" Async="true" AsyncTimeout="30" %>

<%@ Import Namespace="Net.Server" %>
<%@ Import Namespace="Net.Models" %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="content-type" content="text/html; charset=utf-8">
    <meta charset="utf-8">
    <meta name="description" content="Net Search, <%=name%></">
    <title><%=name%>,Net Search</title>

    <link rel="stylesheet" type="text/css" href="Content/semantic.min.css">
    <link rel="stylesheet" type="text/css" href="Content/Search.css">
    <script type="text/javascript" src="js/toTop.js"></script>

    <script>
        function onLoad() {
            toTop('top', false);
            highlight();
        }

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
            div_tip_ID = "tipID_" + objA.id;

            var div_tip = document.getElementById(div_tip_ID);
            div_tip.style.display = "block";
            div_tip.style.left = (obj.offsetLeft + 40) + "px";
            div_tip.style.top = (obj.offsetTop - div_tip.offsetHeight + 160) + "px";
        }
        function hide() {

            intrval = window.setTimeout(function () {
                document.getElementById(div_tip_ID).style.display = "none";
            }, 50);

        }
    </script>
</head>
<body onload="onLoad()">

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

    <div id="s_head">
        <form class="ui large form" action="s.aspx" onsubmit="formsubmit()">
            <div class="ui label input" id="float_banner">
                <div class="ui action input" style="width: 780px">
                    <a href="./"><i class="teal feed icon" style="font-size: 42px"></i></a>
                    <input name="q" value="<%=name%>" required onfocus="formfocus()" />
                    <input id="btnsearch" type="submit" class="ui teal right button" value="Search" />
                    <% if (!string.IsNullOrWhiteSpace(relatedSearchCount))%>
                    <% { %>
                    <span class="nums" style="">找到相关结果约<%=relatedSearchCount %>个</span>
                    <% } %>
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
                <% foreach (var p in pages)
                   {
                %>
                <h3 class="res-title">
                    <a class="stext" target="_blank" href="link.aspx?url=<%=p.Url%>"><%= p.Title%></a>
                </h3>
                <span class="stext"><%=p.Content%> </span>
                <br />
                <% if (p.Tag != null)%>
                <%
                   {
                %>
                <div id="tipID_<%=p.Id%>" class="mingpian-tooltip" onmouseover="show()" onmouseout="this.style.display='none';">
                    <div class="mingpian-title"><%=((SiteInfo)p.Tag).Name%></div>
                    <div class="mingpian-description"><%=((SiteInfo)p.Tag).Description %></div>
                    <div class="mingpian-credit">网站信用 :</div>

                    <% if (((SiteInfo)p.Tag).IsGuard)%>
                    <% { %>
                    <div class="mingpian-planed">
                        <i class="check circle icon"></i>已加入上网保障计划
                    </div>
                    <%}%>
                    <% else %>
                    <% { %>
                    <div class="mingpian-plan">
                        <i class="remove circle icon"></i>未加入上网保障计划
                    </div>
                    <%}%>
                </div>
                <%
                   }
                %>
                <div class="res-linkinfo">
                    <cite><%=p.Url%> </cite>&nbsp;&nbsp;<%=p.Verified%>
                </div>
                <% } %>
            </div>
            <%--left side end--%>

            <%--right side start--%>
            <div id="body_right">
                <% if (pages.Count > 0)%>
                <% { %>

                <div id="so_feb">
                    <dl id="soSafe" class="open">
                        <dt class="" onclick='collapsed()'>
                            <div id="collapse-button">收起<i id="angle" class="angle down icon"></i></div>
                            <span class="safe-guard-logo"></span>
                            <%-- <i class="lock icon"></i>--%>
                            <span>Net Search · 安全保障</span></dt>
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
                            <h3><a href="#" target="_blank" style="font-size: 15px">想在Net Search推广您的产品服务吗？</a></h3>
                            <p class="res-linkinfo">
                                <%--<i class="volume control phone icon"></i>--%>
                                ☎ 4000-123-123<br>
                                <cite>a.bcd.cn</cite>
                            </p>
                        </li>
                    </ul>
                </div>
                <script>
                    function collapsed() {

                        if (document.getElementById('collapse-button').textContent.indexOf('收起') != -1) {
                            document.getElementById('collapse-button').innerHTML = '展开' + "<i id=\"angle\" class=\"angle up icon\"></i>";
                        }
                        else if (document.getElementById('collapse-button').textContent.indexOf('展开') != -1) {
                            document.getElementById('collapse-button').innerHTML = '收起' + "<i id=\"angle\" class=\"angle down icon\"></i>";
                        }
                        var showorhidden = document.getElementById("ad-home");

                        if (showorhidden.style.display == 'none') {
                            showorhidden.style.display = 'block';
                        }
                        else { showorhidden.style.display = 'none' }
                    }
                </script>

                <% } %>
            </div>
            <%--right side end--%>
        </div>
        <% if (pages.Count > 0)%>
        <% { %>
        <div id="page">
            <span><%=pageIndexString%>  </span>
        </div>
        <% } %>
    </div>
    <div>

        <a href="javascript:;" id="top"><i class="teal arrow circle outline up icon" style="margin-left: 20px"></i>
            <br />
            回到顶部</a>
    </div>
    <div id="s_footer">
        <a href="u" target="_blank">意见反馈</a> | <a href="" target="_blank">推广合作</a> | <span>Copyright © Net Search</span>
    </div>
</body>
</html>