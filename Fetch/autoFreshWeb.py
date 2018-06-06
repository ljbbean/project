# coding=utf-8

from selenium import webdriver
from time import sleep
import requests
import sys

browser = webdriver.Chrome("C:\Program Files (x86)\Google\Chrome\Application\chromedriver.exe")

try:
    while 1:
        try:
            button = browser.find_element_by_class_name("button-mod__primary___1tmFA")
            button.click()
            sleep(100)
            break#直接退出
        except:
            browser.get("https://trade.taobao.com/trade/itemlist/list_sold_items.htm")
            qrCode = browser.find_element_by_id("J_QRCodeImg")
            src = qrCode.find_element_by_tag_name("img").get_attribute("src")
            data = {
                'uid':sys.argv[1],
                'url':src
            }
            #判断是否显示，觉得是否已扫描
            #browser.find_element_by_class_name("qrcode-msg")
            #通过url判断是否是https://trade.taobao.com/trade/itemlist/list_sold_items.htm，确定是否在跳转
            #隔1秒判断是否可以抓取
            requests.post("http://localhost:8080/tb_qr", json = data, headers={})#二维码地址传入node
            sleep(100)
            continue
        pass
finally:
    browser.close() 

