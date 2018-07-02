/// <reference path="docs/carpa-vsdoc.js" />

Type.registerNamespace('WebMain'); 

WebMain.WaitAction = function() {  
    WebMain.WaitAction.initializeBase(this);  
}

WebMain.WaitAction.prototype = {  
    initialize: function() {  
        WebMain.WaitAction.callBaseMethod(this, 'initialize');  
    },

    dispose: function() {  
        WebMain.WaitAction.callBaseMethod(this, 'dispose');  
    },

    doButton1Click: function(sender) {
        
    }
}
WebMain.WaitAction.registerClass('WebMain.WaitAction', Sys.UI.PageAction);
