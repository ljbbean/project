/// <reference path="docs/carpa-vsdoc.js" />

Type.registerNamespace('Test001'); 

Test001.SendGoodsAction = function() {  
    Test001.SendGoodsAction.initializeBase(this);  
}

Test001.SendGoodsAction.prototype = {  
    initialize: function() {  
        Test001.SendGoodsAction.callBaseMethod(this, 'initialize');  
    },

    dispose: function() {  
        Test001.SendGoodsAction.callBaseMethod(this, 'dispose');  
    },

    doSend: function(sender) {
        var form = sender.get_form();
        var name = form.name.get_value();
        var code = form.code.get_value();
        form.get_service().SendGood(form.id, name, code, function () {
            form._ok = true;
            form.doOk();
        });
    }
}
Test001.SendGoodsAction.registerClass('Test001.SendGoodsAction', Sys.UI.PageAction);
