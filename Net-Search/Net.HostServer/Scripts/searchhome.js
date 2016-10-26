function onLoad() {
    toTop('top', false);
    highlight();
}

function toTop(id, show) {
    var oTop = document.getElementById(id);
    var bShow = show;
    if (!bShow) {
        oTop.style.display = 'none';
        setTimeout(btnShow, 50);
    }
    oTop.onclick = scrollToTop;
    function scrollToTop() {
        var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
        var iSpeed = Math.floor(-scrollTop / 2);
        if (scrollTop <= 0) {
            if (!bShow) {
                oTop.style.display = 'none';
            }
            return;
        }
        document.documentElement.scrollTop = document.body.scrollTop = scrollTop + iSpeed;
        setTimeout(arguments.callee, 50);
    }
    function btnShow() {
        var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
        if (scrollTop <= 0) {
            oTop.style.display = 'none';
        } else {
            oTop.style.display = 'block';
        }
        setTimeout(arguments.callee, 50);
    }
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
