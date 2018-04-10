/// <reference path="../docs/carpa-vsdoc.js" />

Type.registerNamespace('Test001.Logistics'); 

Test001.Logistics.LogisticsManagerAction = function() {  
    Test001.Logistics.LogisticsManagerAction.initializeBase(this);  
}

Test001.Logistics.LogisticsManagerAction.prototype = {  
    initialize: function() {  
        Test001.Logistics.LogisticsManagerAction.callBaseMethod(this, 'initialize');  
    },

    dispose: function() {  
        Test001.Logistics.LogisticsManagerAction.callBaseMethod(this, 'dispose');  
    },

    doButton1Click: function(sender) {
        
    }
}
Test001.Logistics.LogisticsManagerAction.registerClass('Test001.Logistics.LogisticsManagerAction', Sys.UI.PageAction);
