from selenium import webdriver
from time import sleep

browser = webdriver.Chrome("C:\Program Files (x86)\Google\Chrome\Application\chromedriver.exe")

try:
    while 1:
        try:
            button = browser.find_element_by_class_name("button-mod__primary___1tmFA")
            button.click()
            sleep(60*60)
        except:
            browser.get("https://trade.taobao.com/trade/itemlist/list_sold_items.htm")
            sleep(100)
            continue
        pass
finally:
    browser.close() 

