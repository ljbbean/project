/// <reference path="docs/carpa-vsdoc.js" />

Type.registerNamespace('WebMain'); 

WebMain.PlugListAction = function() {  
    WebMain.PlugListAction.initializeBase(this);  
}

WebMain.PlugListAction.prototype = {  
    initialize: function() {  
        WebMain.PlugListAction.callBaseMethod(this, 'initialize');  
    },

    dispose: function() {  
        WebMain.PlugListAction.callBaseMethod(this, 'dispose');  
    },

    doSearch: function(sender) {
        
    },

    doHeadItemClick: function(sender) {
        
    },

    doItemRender: function(sender) {
        
    },

    doAfterDataBind: function(sender) {
        
    },

    doHeadRender: function(sender) {
        
    },

    doExpand: function(sender) {
        
    }
}
WebMain.PlugListAction.registerClass('WebMain.PlugListAction', Sys.UI.PageAction);
