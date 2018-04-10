/// <reference path="docs/carpa-vsdoc.js" />

Type.registerNamespace('Test001'); 

Test001.AffiliateBillListAction = function() {  
    Test001.AffiliateBillListAction.initializeBase(this);  
}

Test001.AffiliateBillListAction.prototype = {  
    initialize: function() {  
        Test001.AffiliateBillListAction.callBaseMethod(this, 'initialize');  
    },

    dispose: function() {  
        Test001.AffiliateBillListAction.callBaseMethod(this, 'dispose');  
    },

    doButton1Click: function(sender) {
        
    }
}
Test001.AffiliateBillListAction.registerClass('Test001.AffiliateBillListAction', Sys.UI.PageAction);
