<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="Net.Server.Dashboard" EnableSessionState="false" Async="true" AsyncTimeout="30" %>

<%@ Import Namespace="Net.Server" %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="content-type" content="text/html; charset=utf-8">
    <meta charset="utf-8">
    <meta name="description" content="Dashboard">
    <title>Dashboard</title>

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
            justify-content: space-between;
        }

        .flex-item {
            padding: 0px;
            height: 30px;
            margin-top: 0px;
            line-height: 20px;
            color: white;
            font-weight: bold;
            /*font-size: 1em;
            text-align: center;*/
        }
    </style>
</head>
<body>

    <form id="form1" runat="server">

        <div id="body_right">

            <%
                String tcontent = (DateTime.Now - begin).TotalSeconds + "s, "
                        + "MEM:" + (System.GC.GetTotalMemory(false) / 1024 / 1024) + "MB ";
            %>
            <div class="ui segment ">
                Time:
                    <%= tcontent%>
            </div>
            <div class="ui segment ">
                Collected links:
                    <%= processLinksCount%>
                <asp:Button ID="btnProcessLinks" runat="server" class="ui teal right button" Text="Button" OnClick="btnProcessLinks_Click" />
            </div>
            <div class="ui segment">
                Verified site:
                    <%= siteInfoCount%>
                <asp:Button ID="btnSiteInfo" runat="server" class="ui teal right button" Text="Button" OnClick="btnSiteInfo_Click" />
            </div>
            <div class="ui segment">
                Collected pages:
                    <%= sitePageCount%>
                <asp:Button ID="btnSitePage" runat="server" class="ui teal right button" Text="Button" OnClick="btnSitePage_Click" />
            </div>
            <div class="ui segment">
                Searched words:
                    <%= wordsCount%>
                <asp:Button ID="Button1" runat="server" class="ui teal right button" Text="Button" />
            </div>
            <div class="ui segment">
                Linked url:
                    <%= linkedCount%>
                <asp:Button ID="Button2" runat="server" class="ui teal right button" Text="Button" />
            </div>
        </div>

        <div id="s_footer">
            <a href="u" target="_blank">意见反馈</a> | <a href="" target="_blank">推广合作</a> | <span>Copyright © Net Search</span>
        </div>
    </form>
</body>
</html>