/// <reference path="../docs/carpa-vsdoc.js" />

Type.registerNamespace('Test001.Logistics'); 

Test001.Logistics.LogisticsListAction = function() {  
    Test001.Logistics.LogisticsListAction.initializeBase(this);  
}

Test001.Logistics.LogisticsListAction.prototype = {  
    initialize: function() {  
        Test001.Logistics.LogisticsListAction.callBaseMethod(this, 'initialize');  
    },

    dispose: function() {  
        Test001.Logistics.LogisticsListAction.callBaseMethod(this, 'dispose');  
    },

    doButton1Click: function(sender) {
        
    }
}
Test001.Logistics.LogisticsListAction.registerClass('Test001.Logistics.LogisticsListAction', Sys.UI.PageAction);
