﻿<%@ Page Language="C#" EnableSessionState="false" Async="true" AsyncTimeout="30" %>
<%@ Import Namespace="System.Collections.Generic" %>

<%
    List<String> discoveries = new List<String>();

%>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    <meta name="description" content="iBoxDB NoSQL Database Full Text Search Server FTS" />
    <title>Full Text Search Server</title>

    <link rel="stylesheet" type="text/css" href="Content/semantic.min.css" />

    <style>
        td {
            white-space: nowrap;
            overflow: hidden;
        }

        body {
            margin-top: 100px;
            overflow: hidden;
        }

            body > .grid {
            }

        .column {
            max-width: 60%;
        }
    </style>
</head>
<body>
    <div class="ui middle aligned center aligned grid">
        <div class="column">

            <h2 class="ui teal header">
                <span style="font-size: 40px"><i class="teal feed icon" style="font-size: 42px"></i>Full Text Search Server</span>
            </h2>
            <form class="ui large form" action="s.aspx" onsubmit="formsubmit()">
                <div class="ui label input">
                    <div class="ui action input">
                        <input name="q" value="" onfocus="formfocus()" required />
                        <input id="btnsearch" type="submit" class="ui teal right button big"
                            value="Search" />
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

            <div class="ui message" style="text-align: left">
                Input [KeyWord] to search,  input [URL] to index
                <br />
                Input [delete URL] to delete.

                <br />
                Recent Searches:<br />
      <%--          <%
                    foreach (String str in SearchResource.searchList)
                    {

                    %> <a href="s.aspx?q=<%=str.Replace("#", "%23") %>"><%=str%></a>. &nbsp;

                <%
                        }
                    %>--%>

                <br>
                Recent Records:<br>
         <%--       <%
                    foreach (String str in SearchResource.urlList)
                    {
                    %>
                <a href="<%=str%>" target="_blank"><%=str%></a>.
                <br>
                <%
                        }
                    %>--%>

                <br />
                <a href="./">Refresh Discoveries</a>:&nbsp;

                <%
                    foreach (String str in discoveries)
                    {

                    %> <a href="s.aspx?q=<%=str.Replace("#", "%23") %>"><%=str%></a>. &nbsp;

                <%
                        }
                    %>
            </div>
        </div>
    </div>
</body>
</html>