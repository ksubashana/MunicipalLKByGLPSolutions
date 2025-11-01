window.ConnectionStatus = (function(){
    let dotNetRef = null;
    function init(ref){
        dotNetRef = ref;
        window.addEventListener('blazor:connectionDown', onDown);
        window.addEventListener('blazor:connectionUp', onUp);
    }
    function onDown(){
        if(dotNetRef){ dotNetRef.invokeMethodAsync('NotifyConnectionDown'); }
    }
    function onUp(){
        if(dotNetRef){ dotNetRef.invokeMethodAsync('NotifyConnectionUp'); }
    }
    function reload(){ location.reload(); }
    return { init, reload };
})();