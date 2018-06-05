var http = require("http");
var express = require("express");
var socketIo = require("socket.io");
var app = new express();
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
        }
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
            io.to(ncid).emit("exec", { type: 'failed', msg: '消息发送者和对应的登录信息不一致，无法继续操作' })
            return false;
        }
        return true;
    }

    function isOnline(id) {
        return io.eio.clients[id] ? true : false
    }

    clientSocket.on("login", (data) => {
        let uid = data.uid
        let cid = userMap[uid]
        let ncid = clientSocket.id//新客户端ID
        if (cid == clientSocket.id) {
            io.to(ncid).emit("exec", { type: 'reLogin' })
            return;
        }
        if (cid && isOnline(cid)) {//已经登录，并且之前的连接还存在，询问是否退出之前的登录信息
            let id = Date.now()
            let oldUser = users[cid]
            oldUser[id] = {}
            //注册回应回调函数
            oldUser[id].ok = (rdata) => {
                users[ncid] = oldUser
                userMap[uid] = ncid
                delete users[cid]
                io.to(cid).emit("exec", { type: 'exit' })
                io.to(ncid).emit("exec", { type: 'loginSuccess' })
            }
            oldUser[id].no = (rdata) => {
                io.to(ncid).emit("exec", { type: 'refuse' })//请求被拒绝
            }
            io.to(cid).emit("ask", { id: id, msg: "有其他客户端接入登录，是否退出当前登录？" })
        } else {
            users[ncid] = {}
            userMap[uid] = ncid
            io.to(ncid).emit("exec", { type: 'loginSuccess' })
        }
    })

    clientSocket.on("sure", (data) => {
        let user = users[clientSocket.id]
        let fn = user[data.id][data.msg];
        if (fn) {
            fn(data)
        }
        delete user[data.id]
    })

    clientSocket.on("exit", (data) => {
        if (!checkUser(data.uid)) {
            return;
        }
        delete users[clientSocket.id]
        delete userMap[data.uid]
    })

    clientSocket.on("sendMsg", function (data) {
        if (!checkUser(data.uid)) {
            return;
        }

        if (!data.touid) {//没有接受人，所以广播
            var ndata = {
                fuid: data.uid,//消息来源
                date: Date.now(),//发送时间
                msg: data.msg//消息内容
            }
            //广播
            clientSocket.broadcast.emit("receiveMsg", ndata);
            return;
        }

        let toucid = userMap[data.touid]
        if (toucid && isOnline(toucid)) {
            io.to(toucid).emit("receiveMsg", {
                fuid: data.uid,//消息来源
                date: Date.now(),//发送时间
                msg: data.msg//消息内容
            })
            return
        }
        //回发给自己
        clientSocket.emit("exec", { type: 'failed', msg: '未找到对应的接收方，可能对象已下线' })
    })
});