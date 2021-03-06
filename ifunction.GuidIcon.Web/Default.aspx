﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ifunction.GuidIcon.Web._Default" %>

<!DOCTYPE>

<html>
<head>
    <title>GICON Demo Web</title>
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.11.1.min.js"></script>
    <script type="text/javascript">

        function newIcon(instance, guid, width, t) {
            instance = $(instance);
            var src = instance.attr("src");
            instance.attr("src", "gicon.ashx?hash=" + (guid || "") + "&size=" + (width || 0) + "&t=" + (t || ""));
            instance.attr("width", width).attr("height", width);
        }

        $(document).ready(function () {

            $("#generate").click(function () {
                newIcon($("#image"), $("#hash").val(), $("#width").val());
            });

            $("#random").click(function () {
                $("#hash").val((new Date()).getTime());
                $("#generate").click();
            });
        });
    </script>

    <style>
        body {
            font-family: arial;
            font-size: 14px;
            font-weight: bold;
        }

        label {
            width: 50px;
        }

        #hash {
            font-family: arial;
            font-size: 14px;
            font-weight: bold;
            width: 300px;
        }

        #description {
            font-weight: normal;
            font-size: 12px;
        }

        #allright {
            position: absolute;
            height: 40px;
            left: 0;
            right: 0;
            bottom: 0;
        }
    </style>
</head>
<body>
    <div>
        <label>Input:</label><input type="text" id="hash" placeholder="< Hash > ..." />
    </div>
    <div>
        <label>Width:</label><select id="width">
            <option value="256">256 x 256</option>
            <option value="400">400 x 400</option>
            <option value="512">512 x 512</option>
        </select>
        <input type="button" id="generate" value="Generate" />
        <input type="button" id="random" value="Random" />
    </div>
    <div id="description">
        Input here could be GUID, SHA1, MD5 or any other hex string consisted by 32 hex charset.
    Sample:
    <ul>
        <li>03348541-a4b1-4a42-9cf1-90c538b01ecd</li>
        <li>03348541-A4B1-4A42-9CF1-90C538B01ECD</li>
        <li>03348541a4b14a429cf190c538b01ecd</li>
        <li>03348541A4B14A429CF190C538B01ECD</li>
    </ul>
        Or any other string. System would hash it using MD5 in UTF-8 encoding.
    </div>
    <div>
        <image id="image" />

        <div id="allright">
            <div>Open Source Project: gicon</div>
            <div>Source Code: <a href="http://gicon.codeplex.com/" target="_blank">Codeplex</a> or <a href="http://github.com/rynnwang/gicon/" target="_blank">Github</a></div>
        </div>
    </div>
</body>
</html>
