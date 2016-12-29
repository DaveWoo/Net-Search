function toTop(id,show){
    var oTop = document.getElementById(id);
    var bShow = show;
    if(!bShow){
        oTop.style.display = 'none';
        setTimeout(btnShow,50);
    }
    oTop.onclick = scrollToTop;
    function scrollToTop(){
        var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
        var iSpeed = Math.floor(-scrollTop/2);
        if(scrollTop <= 0){
            if(!bShow){
                oTop.style.display = 'none';
            }
            return;
        }
        document.documentElement.scrollTop = document.body.scrollTop = scrollTop + iSpeed;
        setTimeout(arguments.callee,50);
    }
    function btnShow(){
        var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
        if(scrollTop <= 0 ){
            oTop.style.display = 'none';
        }else{
            oTop.style.display = 'block';
        }
        setTimeout(arguments.callee,50);
    }
}