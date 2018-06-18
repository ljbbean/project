var http = require("http");
var express = require("express");
var socketIo = require("socket.io");
var bodyParser = require("body-parser")
var cp = require("child_process")
var app = new express();

function getCurrentDate() {
    let date = new Date();
    return `${date.getFullYear()}-${date.getMonth()}-${date.getDate()} ${date.getHours()}:${date.getMinutes()}:${date.getSeconds()} ${date.getMilliseconds()}`
}

app.use(bodyParser.json())
app.post("/tb_qr", function (req, res) {
    let data = req.body
    let cid = userMap[data.uid]
    if (cid) {
        let socket = io.to(cid)
        let msg = data.msg.length > 0 ? data.msg : '二维码成功返回,可以用千牛或淘宝手机端扫描授权抓取了'
        socket.emit("exec", { type: 'doing', msg: msg, date: getCurrentDate(), url: data.url })
    }
    res.end()
})

app.post("/sendData", function (req, res) {
    let data = req.body
    res.end()
})

app.get("/getFilter", function(req, res){
    res.send({
        url:"https://trade.taobao.com/trade/security/auth_user_info.htm"
    })
    res.end()
})
//确认python 发起用户ID是否存在，如果不存在，则关闭浏览器
app.post("sureExitId",function(req, res){
    let data = req.body
    let cid = userMap[data.uid]
    if (cid && io.eio.clients[cid]) {
        res.end("true")
        return;
    }
    res.end("false")
})
var server = http.createServer(app);
var io = new socketIo(server);
server.listen(8080);
console.log('server listen 8080');
var users = {
    /*
    clientSocketid:{
        login:{
            //id = Datenow()
            id:{
                ok:fn,
                no:fn
            }
        },
        needAsk:true/false
    }
     */
};
var userMap = {
    /*
    uid:clientSocket.id
    */
};

//每个客户端socket连接时都会触发 connection 事件
io.on("connection", function (clientSocket) {

    function checkUser(uid) {
        if (userMap[uid] != clientSocket.id) {
            clientSocket.emit("exec", { type: 'failed', msg: '消息发送者和对应的登录信息不一致，无法继续操作', date: getCurrentDate() })
            return false;
        }
        return true;
    }

    function isOnline(id) {
        return io.eio.clients[id] ? true : false
    }

    function getData(data) {
        if (Object.prototype.toString.call(data) === "[object String]") {
            return JSON.parse(data);
        }
        return data;
    }

    clientSocket.on("login", (data) => {
        data = getData(data);
        let uid = data.uid
        let cid = userMap[uid]
        let ncid = clientSocket.id//新客户端ID
        if (cid == clientSocket.id) {
            io.to(ncid).emit("exec", { type: 'reLogin', date: getCurrentDate() })
            return;
        }
        
        if (cid && isOnline(cid) && users[cid].needAsk !== false) {//已经登录，并且之前的连接还存在，询问是否退出之前的登录信息
            let id = Date.now()
            let oldUser = users[cid]
            oldUser[id] = {}
            //注册回应回调函数
            oldUser[id].ok = (rdata) => {
                users[ncid] = oldUser
                userMap[uid] = ncid
                delete users[cid]
                io.to(cid).emit("exec", { type: 'exit', date: getCurrentDate() })
                io.to(ncid).emit("exec", { type: 'loginSuccess', date: getCurrentDate() })
            }
            oldUser[id].no = (rdata) => {
                io.to(ncid).emit("exec", { type: 'refuse', date: getCurrentDate() })//请求被拒绝
            }
            io.to(cid).emit("ask", { id: id, msg: "有其他客户端接入登录，是否退出当前登录？" })
        } else {
            users[ncid] = {
                needAsk : data.needAsk
            }
            userMap[uid] = ncid
            io.to(ncid).emit("exec", { type: 'loginSuccess', date: getCurrentDate() })
        }
    })

    clientSocket.on("sure", (data) => {
        data = getData(data);
        let user = users[clientSocket.id]
        let fn = user[data.id][data.msg];
        if (fn) {
            fn(data)
        }
        delete user[data.id]
    })

    clientSocket.on("exit", (data) => {
        data = getData(data);
        if (!checkUser(data.uid)) {
            return;
        }
        delete users[clientSocket.id]
        delete userMap[data.uid]
    })

    clientSocket.on("getTB_QR", function (data) {
        cp.exec(`python ${__dirname}\\..\\Fetch\\autoFreshWeb.py ` + data.uid, { windowsHide: false }, (err, stdout, stderr) => {
            clientSocket.emit("exec", { type: 'failed', msg: '获取二维码错误' + stderr, date: getCurrentDate() })
        })
        clientSocket.emit("exec", { type: 'doing', msg: '正在获取授权二维码，请等待', date: getCurrentDate() })
    })

    clientSocket.on("sendMsg", function (data) {
        data = getData(data);
        if (!checkUser(data.uid)) {
            return;
        }

        if (!data.touid) {//没有接受人，所以广播
            // var ndata = {
            //     fuid: data.uid,//消息来源
            //     date: Date.now(),//发送时间
            //     msg: data.msg//消息内容
            // }
            // //广播
            // clientSocket.broadcast.emit("receiveMsg", ndata);
            clientSocket.emit("exec", { type: 'failed', msg: '未设置消息的接收方  touid 数据', date: getCurrentDate() })
            return;
        }

        let toucid = userMap[data.touid]
        if (toucid && isOnline(toucid)) {
            io.to(toucid).emit("receiveMsg", {
                fuid: data.uid,//消息来源
                date: getCurrentDate(),//发送时间
                msg: data.msg//消息内容
            })
            return
        }
        //回发给自己
        clientSocket.emit("exec", { type: 'failed', msg: '未找到对应的接收方，可能对象已下线', date: getCurrentDate() })
    })

    clientSocket.on("postDataRequest", function(data){
        data = getData(data);
        if (!checkUser(data.uid)) {
            return;
        }
        let toucid = userMap[data.touid]
        if (toucid && isOnline(toucid)) {
            io.to(toucid).emit("postDataRequest", {
                fuid: data.uid,//消息来源
                date: getCurrentDate(),//发送时间
                msg: data.msg//消息内容
            })
            return
        }
        //回发给自己
        clientSocket.emit("exec", { type: 'failed', msg: '未找到对应的接收方，可能对象已下线', date: getCurrentDate() })
    })

    clientSocket.on("postDataSure", function(data){
        data = getData(data);
        if (!checkUser(data.uid)) {
            return;
        }
        let toucid = userMap[data.touid]
        if (toucid && isOnline(toucid)) {
            io.to(toucid).emit("postDataSure", {
                fuid: data.uid,//消息来源
                date: getCurrentDate(),//发送时间
                msg: data.msg//消息内容
            })
            return
        }
        //回发给自己
        clientSocket.emit("exec", { type: 'failed', msg: '未找到对应的接收方，可能对象已下线', date: getCurrentDate() })
    })
});