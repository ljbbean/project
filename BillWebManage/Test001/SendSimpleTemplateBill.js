/// <reference path="docs/carpa-vsdoc.js" />

Type.registerNamespace('Test001'); 

Test001.SendSimpleTemplateBillAction = function() {  
    Test001.SendSimpleTemplateBillAction.initializeBase(this);  
}

Test001.SendSimpleTemplateBillAction.prototype = {  
    initialize: function() {  
        Test001.SendSimpleTemplateBillAction.callBaseMethod(this, 'initialize');  
    },

    dispose: function() {  
        Test001.SendSimpleTemplateBillAction.callBaseMethod(this, 'dispose');  
    },

    doSure: function (sender, e) {
        var form = sender.get_form();
        if (confirm("确认已签收？")) {
            var id = e._item.bid.get_text();
            form.get_service().Sure(id, function () {
                e._item.dataImage1.set_visible(false)
            });
        }
    }
}
Test001.SendSimpleTemplateBillAction.registerClass('Test001.SendSimpleTemplateBillAction', Sys.UI.PageAction);
