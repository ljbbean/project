(function (w) {
    var $1 = document.getElementById;
    $ = function(id) { return $1.call(document, id);}
    var captionArray = [{
        width: 70,
        title: '图片'
    }, {
        width: 400,
        title: '插件信息'
    }, {
        width: 100,
        title: '视频样式'
    }, {
        width: 100,
        title: '图片预览'
    }, {
        width: 100,
        title: '版本号'
    }, {
        width: 100,
        title: '更新日期'
    }, {
        width: 100,
        title: '访问量'
    }, {
        width: 100,
        title: ' '
    }];
    function getItem(itemData, headers, innerData) {
        var label = itemData.label;
        var label1 = "";
        var label2 = "";
        var label3 = "";
        if (label) {
            var labelArray = label.split(',');
            label1 = labelArray[0] == undefined ? "" : labelArray[0];
            label2 = labelArray[1] == undefined ? "" : labelArray[1];
            label3 = labelArray[2] == undefined ? "" : labelArray[2];
        }
        var text = "下载";
        for (var i in innerData) {
            if (innerData[i].ID == itemData.id) {
                text = "更新";
                if (innerData[i].UpdateDate == itemData.pupdatedate) {
                    text = "打开";
                }
            }
        }

        var item = `<div class="item">
                <div style="width:${headers[0].width}px;">
                    <img src="${itemData.img}" class="img" width="60px" height="60px" />
                </div>
                <div class="content">
                    <div class="caption" style="width:${headers[1].width}px;">
                        <div style="margin:0px 0px -5px 0px;float:none;">
                            <span class="name">${itemData.name}</span>
                            <span class="label1" style="display:${label1 != "" ? '' : 'none'}">${label1}</span>
                            <span class="label2" style="display:${label2 != "" ? '' : 'none'}">${label2}</span>
                            <span class="label3" style="display:${label3 != "" ? '' : 'none'}">${label3}</span>
                        </div>
                        <div>
                            <p class="des">${itemData.des}</p>
                        </div>
                    </div>
                    <div class="column" style="width:${headers[2].width}px;">
                        <a href="${itemData.video}" class="iconfont icon-shipin">视频演示</a>
                    </div>
                    <div class="column" style="width:${headers[3].width}px;">
                        <span tag="${itemData.ppics}" class="iconfont icon-msnui-thumb">图片预览</span>
                    </div>
                    <div class="column" style="width:${headers[4].width}px;">
                        <span>${itemData.version}</span>
                    </div>
                    <div class="column" style="width:${headers[5].width}px;">
                        <span>${itemData.pupdatedate}</span>
                    </div>
                    <div class="column" style="width:${headers[6].width}px;">
                        <span>${itemData.view}</span>
                    </div>
                    <div class="column" style="width:${headers[7].width}px;">
                        <div class="progressBtn" onclick="window.plugs.itemClick('${itemData.pdownpath}', '${itemData.id}')">
                            <span id="SpanProgress${itemData.id}" style="width: 0px; display:none;">
                                <span>
                                </span>
                            </span>
                            <div style="position:absolute; width:80px; margin:5px 0px 5px -5px;">
                                <span id="SpanIncefont${itemData.id}" class="iconfont icon-Versionupdaterule-copy" style="font-size:14px;"></span>
                                <span style="font-size:14px;" id="SpanText${itemData.id}">${text}</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>`;
        return item;
    }

    function getHeaders(headers) {
        var str = "";
        for (var i in headers) {
            var header = headers[i];
            str = str + `<div class="header" style="width:${header.width}px;">
                    ${header.title}
                </div>`;
        }
        return str;
    }

    function itemClick(pdownpath, id) {
        var tid = "SpanText"+ id;
        var iid = "SpanIncefont" + id;
        var way = $(tid).innerHTML;
        try {
            switch (way) {
                case '打开':
                    innerJs.plugOpen(id);
                    break;
                case '下载':
                case '更新':
                case '重试':
                    innerJs.plugDownLoad(id, pdownpath);
                    downProcess(id);
                    $(tid).className = "iconfont icon-time";
                    $(iid).style.display = "none";
                    break;
            }
        } catch (e) {
            alert(e);
        }
    }

    function downProcess(id) {
        window.setTimeout(function(){
            var data = JSON.parse(innerJs.downLoadStatus(id)) || {ok:true};
            
            var pid = "SpanProgress"+ id;
            var tid = "SpanText"+ id;
            if(data.error){
                $(tid).innerText = "重试";
            } else if(data.ok){
                //下载完成
                $(tid).innerText = "打开";
                $(pid).style.display = "none";
            } else {
                if(data.percentage == "100%"){
                    $(tid).innerText = "预热中";
                    $(pid).style.display = "none";
                } else {
                    //更新进度data.Max  data.Downed
                    $(pid).style.width = data.percentage;
                    $(pid).style.display = "";
                    $(tid).innerText = "下载中";
                }
                downProcess(id);
            }
        }, 100);
    }

    function loadItems() {
        var headers = document.getElementById("headers");
        headers.innerHTML = getHeaders(captionArray);

        var details = $("details");
        details.innerHTML = "数据加载中……";

        new cefnet().post("WebMain/WebMain.PlugList.ajax/GetItems", function (data) {
            var str = "";
            var innerData = getInnerData();
            for (var i = 0, count = data.length; i < count; i++) {
                str = str + getItem(data[i], captionArray, innerData);
            }
            details.innerHTML = str;
        }, (error) => {
            details.innerHTML = error;
        });
    }

    function getInnerData() {
        try {
            var data = innerJs.getPlugsInfo();
            return JSON.parse(data);
        } catch (e) {
            return [];
        }
    }
    
    w.plugs = {
        getInnerData : getInnerData,
        loadItems : loadItems,
        itemClick:itemClick
    }
})(window);