/// <reference path="docs/carpa-vsdoc.js" />

Type.registerNamespace('WebMain'); 

WebMain.PlugManagerAction = function() {  
    WebMain.PlugManagerAction.initializeBase(this);  
}

WebMain.PlugManagerAction.prototype = {
    initialize: function () {
        WebMain.PlugManagerAction.callBaseMethod(this, 'initialize');
    },

    dispose: function () {
        WebMain.PlugManagerAction.callBaseMethod(this, 'dispose');
    },

    doImageClick: function (sender) {
        var form = sender.get_form();
        form.FileIcon.get_element().click();
        this.imageSender = sender;
    },

    doImageSelecteChanged: function (sender) {
        var image = this.imageSender;
        Sys.UI.Controls.FileUpload.submit(sender, function (t, result) {
            if (result == "") {
                return;
            }
            image.set_src(result);
        }, Function.createDelegate(this, this.doFaild));
    },

    doVideoSelecteChanged: function (sender) {
        Sys.UI.Controls.FileUpload.submit(sender, function (s, result) {
            form.FileVideo.set_tag(result);
        }, Function.createDelegate(this, this.doFaild));
    },

    doZipSelecteChanged: function (sender) {
        var form = sender.get_form();
        Sys.UI.Controls.FileUpload.submit(sender, function (s, result) {
            form.FileZip.set_tag(result);
        }, Function.createDelegate(this, this.doFaild));
    },

    doFaild: function (sender, message) {
        sender.set_text("");
        $common.alert(message);
    },

    doSave: function (sender) {
        var form = sender.get_form();
        var data = form.saveData();
        data.picon = form.picon.get_src();
        if (!data.picon) {
            alert('请上传插件图标');
            return;
        }
        data.ppic1 = form.ppic1.get_src();
        data.ppic2 = form.ppic2.get_src();
        data.ppic3 = form.ppic3.get_src();
        data.pdownpath = form.FileZip.get_tag() ? form.FileZip.get_tag() : form.hpdownpath.get_value();
        data.pvideo = form.FileVideo.get_tag() ? form.FileVideo.get_tag() : form.hpvideo.get_value();
        delete data.FileZip;
        delete data.FileVideo;
        form.get_service().SavePlugData(data, function () {
            form.doOk();
        });
    }
}
WebMain.PlugManagerAction.registerClass('WebMain.PlugManagerAction', Sys.UI.PageAction);
