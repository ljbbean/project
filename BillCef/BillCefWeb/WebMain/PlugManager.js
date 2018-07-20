/// <reference path="docs/carpa-vsdoc.js" />

Type.registerNamespace('WebMain'); 

WebMain.PlugManagerAction = function() {  
    WebMain.PlugManagerAction.initializeBase(this);  
}

WebMain.PlugManagerAction.prototype = {  
    initialize: function() {  
        WebMain.PlugManagerAction.callBaseMethod(this, 'initialize');  
    },

    dispose: function() {  
        WebMain.PlugManagerAction.callBaseMethod(this, 'dispose');  
    },

    doButton1Click: function(sender) {
        
    }
}
WebMain.PlugManagerAction.registerClass('WebMain.PlugManagerAction', Sys.UI.PageAction);
