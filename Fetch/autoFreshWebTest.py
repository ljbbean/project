# coding=utf-8

from selenium import webdriver
from time import sleep
#import requests
import sys
import threading

global buttonClassName
global J_QRCodeImg
global listUrl
global qrcode_msg
global browser

buttonClassName = "button-mod__primary___1tmFA"
J_QRCodeImg = "J_QRCodeImg"
listUrl = "https://trade.taobao.com/trade/itemlist/list_sold_items.htm"
qrcode_msg = 'qrcode-msg'
browser = webdriver.Chrome("C:\Program Files (x86)\Google\Chrome\Application\chromedriver.exe")

def getStatus():
    if isExitElement(browser, qrcode_msg):
        qrCodeMsg = browser.find_element_by_class_name(qrcode_msg)
        qrCodeMsgVisible = qrCodeMsg.get_attribute("visible")
        if strcmp(qrCodeMsgVisible,""):
            #发送通知，正处于已扫描待确认阶段
            print '发送通知，正处于已扫描待确认阶段'
            return

    if browser.current_url.lower().startswith(listUrl):
        #已跳转到对应页面，准备抓取数据
        print '已跳转到对应页面，准备抓取数据'
        search(browser)
        pass

    global timer
    timer = threading.Timer(1, getStatus)
    timer.start()
    pass

browser.get(listUrl)

timer = threading.Timer(1, getStatus)
timer.start()

#判断是否在浏览器存在某控件
def isExitElement(browser, key):
    try:
        browser.find_element(key)
    except:
        return False
    return True

#执行浏览器查询操作
def search(browser):
    button = browser.find_element_by_class_name(buttonClassName)
    button.click()
    sleep(100)
    browser.close()

# qrCode = browser.find_element_by_id("J_QRCodeImg")
# src = qrCode.find_element_by_tag_name("img").get_attribute("src")


