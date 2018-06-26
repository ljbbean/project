/// <reference path="docs/carpa-vsdoc.js" />

Type.registerNamespace('WebMain'); 

WebMain.StatisticsPageAction = function() {  
    WebMain.StatisticsPageAction.initializeBase(this);  
}

WebMain.StatisticsPageAction.prototype = {
    initialize: function () {
        WebMain.StatisticsPageAction.callBaseMethod(this, 'initialize');

    },

    chatItemClick: function (sender) {
        var buttonArray = ["ddzs", "zfje", "tkds", "ddly"];
        var form = sender.get_form();
        var _this = this;
        form.get_service().GetData(form.area.get_value(), sender.get_tag(), function (data) {
            _this.get_form().mychart.get_echart().setOption(data, true);
        });
        for (var i in buttonArray) {
            form[buttonArray[i]].get_element().className = "buttonNoSelected";
        }
        sender.get_element().className = "button";
    },

    doStaticsLoaded: function (sender) {
        var _this = this;
        this.get_service().GetData(sender.area.get_value(), 0, function (data) {
            if (!data.series || data.series.length == 0) {
                data = {
                    title: {
                        text: '商品销量数量'
                    },
                    legend: {
                        data: []
                    },
                    xAxis: { data: [] },
                    yAxis: [{ type: 'value'}],
                    series: [
                        {
                            name: "商品1",
                            type: "line",
                            data: [],
                            itemStyle: {
                                normal: {
                                    label: { show: true }
                                }
                            }
                        }]
                }
            }
            _this.get_form().mychart.get_echart().setOption(data, true);
        });
    }
}
WebMain.StatisticsPageAction.registerClass('WebMain.StatisticsPageAction', Sys.UI.PageAction);
