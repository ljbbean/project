# coding=utf-8

from selenium import webdriver
from time import sleep
import requests
import sys
import threading


class TBBrower:
    qrcode_img = "qrcode-img"  # 二维码名称
    listUrl = "https://trade.taobao.com/trade/itemlist/list_sold_items.htm"
    sleepTime = 60  # 默认停顿60秒
    login_switch = "login-switch"  # 切换二维码名称
    qrcode_login_ok = "qrcode-login-ok"  # 二维码已被扫描
    qrcode_login_error = "qrcode-login-error"  # 二维码过期
    J_QRCodeRefresh = "J_QRCodeRefresh"  # 刷新二维码
    browser = None  # 浏览器对象
    sendedQR = False  # 已经发生二维码
    sendedWaiting = False  # 已经发生等待确认信息
    buttonName = "button-mod__primary___1tmFA"  # 搜索按钮
    url = "http://localhost:8080/tb_qr"

    def __init__(self):
        self.browser = webdriver.Chrome("C:\Program Files (x86)\Google\Chrome\Application\chromedriver.exe")
        self.browser.get(self.listUrl)

    def sendMsgToNode(self, msg, url):
        data = {
            'uid': sys.argv[1],
            'msg': msg,
            'url':url
        }
        requests.post(self.url, json=data, headers={})  # 二维码地址传入node

    # 判断是否在浏览器存在某控件
    def isExitElementByClassName(self, browser, key):
        try:
            self.browser.find_element_by_class_name(key)
        except:
            return False
        return True

    # 执行浏览器查询操作
    def search(self, browser):
        button = self.browser.find_element_by_class_name(self.buttonName)
        button.click()
        sleep(10)
        self.browser.close()
        self.sleepTime = 1

    def getQRSrc(self):
        if self.isExitElementByClassName(self.browser, self.qrcode_img):
            qrCode = self.browser.find_element_by_class_name(self.qrcode_img)
            src = qrCode.find_element_by_tag_name("img").get_attribute("src")
            if self.sendedQR:
                return
            self.sendMsgToNode('', src)
            self.sendedQR = True
            # 发送二维码地址
            print u'二维码地址：', src
        else:
            # 点击右上角的按钮，切换出二维码
            if self.isExitElementByClassName(self.browser, self.login_switch):
                self.browser.find_element_by_class_name(
                    self.login_switch).click()
                self.getQRSrc()

    def getStatus(self):
        if self.browser.current_url.lower().startswith(self.listUrl):
            # 已跳转到对应页面，准备抓取数据
            print u'已跳转到对应页面，准备抓取数据'
            self.sendMsgToNode('已跳转到对应页面，准备抓取数据', '')
            self.search(self.browser)
            return  # 跳出线程
        else:
            # 二维码已过期
            if self.isExitElementByClassName(self.browser, self.qrcode_login_error):
                # 二维码已过期，请等待最新的二维码
                print u'二维码已过期，请等待最新的二维码'
                self.sendMsgToNode('二维码已过期，请等待最新的二维码', '')
                self.browser.find_element_by_class_name(
                    self.J_QRCodeRefresh).click()
                sleep(2)
                self.sendedQR = False
                self.sendedWaiting = False
            if self.isExitElementByClassName(self.browser, self.qrcode_login_ok):
                if not self.sendedWaiting:
                    # 发送通知，正处于已扫描待确认阶段
                    self.sendedWaiting = True
                    print u'处于已扫描待确认阶段'
                    self.sendMsgToNode('处于已扫描待确认阶段', '')
            else:
                self.getQRSrc()

        global timer
        timer = threading.Timer(1, self.getStatus)
        timer.start()


tbBrower = TBBrower()

timer = threading.Timer(1, tbBrower.getStatus)
timer.start()

while tbBrower.sleepTime == 60:  # 等待线程判断
    sleep(tbBrower.sleepTime)
