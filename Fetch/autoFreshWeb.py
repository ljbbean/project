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
            sleep(60*60)
        except:
            browser.get("https://trade.taobao.com/trade/itemlist/list_sold_items.htm")
            qrCode = browser.find_element_by_id("J_QRCodeImg")
            src = qrCode.find_element_by_tag_name("img").get_attribute("src")
            data = {
                'uid':sys.argv[1],
                'url':src
            }
            requests.post("http://localhost:8080/tb_qr", json = data, headers={})#二维码地址传入node
            sleep(100)
            continue
        pass
finally:
    browser.close() 

