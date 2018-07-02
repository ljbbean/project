var __skinData = {
    common: {
        colors: {
            controlText: "black", controlBg: "#ffffff", windowBg: "white",
            highlightBg: "#E1EBFF", highlightText: "#ff0000",
            hoverBg: "#FDFCF4", hoverText: "#000000", activeBg: "#E2F7FF",
            disabledBg: "#fafafa", disabledText: "#9BBBE0", buttonHoverText: "#000000", formOverlayBg: "#000"
        },
        buttonColors: {
            active: { start: "#ffefe6", end: "#ff9c28", middle: "#eefeff" }
        },
        buttonBg: { image: {
            imageName: "", imageCount: 1, imageWidth: 1, imageHeight: 23
        }
        },
        activeButtonBg: {
            image: {
                imageName: "", imageCount: 2, imageWidth: 1, imageHeight: 23
            }
        },
        loading: { width: 107, height: 53, image: { imageName: "loading.gif"} },
        hintBox: {
            border: "1px solid #FF6600", bgColor: "#FDF6DF", color: "#FF6600",
            padding: "2px 4px 2px 4px", spacing: 10, borderColor: "#FF6600"
        },
        hintBoxTop: {
            image: { imageName: "hintBoxTop.gif", imageCount: 1, imageWidth: 400, imageHeight: 9 }
        },
        hintBoxLeft: {
            image: { imageName: "hintBoxLeft.gif", imageCount: 1, imageWidth: 9, imageHeight: 200 }
        },
        hintBoxRight: {
            image: { imageName: "hintBoxRight.gif", imageCount: 1, imageWidth: 9, imageHeight: 200 }
        },
        hintBoxClose: {
            image: { imageName: "hintBoxClose.gif", imageCount: 1, imageWidth: 13, imageHeight: 13 }
        },
        hintBlock: {
            border: "1px solid #FF6600", bgColor: "#FDF6DF",
            contentMargin: "5px 2px 2px 5px", closeButtonMargin: "3px 3px 0px 0px", borderColor: "#FF6600"
        },
        hintAngleUpLeft: {
            image: { imageName: "hintAngleUpLeft.gif", imageCount: 1, imageWidth: 9, imageHeight: 9 }
        },
        hintAngleUpRight: {
            image: { imageName: "hintAngleUpRight.gif", imageCount: 1, imageWidth: 9, imageHeight: 9 }
        }
    },

    editors: {
        pwdButton: { image: {
            imageName: "pwdButton.gif", imageCount: 2, imageWidth: 19, imageHeight: 19
        }
        },
        comboButton: { image: {
            imageName: "comboButton.gif", imageCount: 4, imageWidth: 18, imageHeight: 19
        }
        },
        ellipsisButton: { image: {
            imageName: "ellipsisButton.gif", imageCount: 4, imageWidth: 18, imageHeight: 19
        }
        },
        spinUp: { image: {
            imageName: "spinUp.gif", imageCount: 4, imageWidth: 18, imageHeight: 10
        }
        },
        spinDown: { image: {
            imageName: "spinDown.gif", imageCount: 4, imageWidth: 18, imageHeight: 9
        }
        },
        calcButton: { image: {
            imageName: "calcButton.gif", imageCount: 4, imageWidth: 18, imageHeight: 19
        }
        }
    },

    form: {
        properties: {
            captionHeight: 22, smallCaptionHeight: 17
        },
        captionBg: { image: { imageName: ""} },
        buttonClose: { image: {
            imageName: "buttonClose.png", imageCount: 4, imageWidth: 17, imageHeight: 17
        }
        },
        smallButtonClose: { image: {
            imageName: "smallButtonClose.gif", imageCount: 4, imageWidth: 15, imageHeight: 15
        }
        }
    },

    grid: {
        properties: {
            borderSize: 1, borderPadding: 3, rowNoColumnWidth: 40
        },
        colors: {
            headerLine: "#B7B7B7;box-shadow:1px 2px 2px #fff",
            normalBg: "#ffffff", normalText: "black",
            evenBg: "#F7F5F5", evenText: "black", // Å¼ÊýÐÐ
            selectedBg: "#e2f3fd", selectedText: "#000000",
            hoverBg: "#FDFCF4", hoverText: "#000000",
            disabledBg: "#F6FCFF", disabledText: "#000000"
        },
        headerColors: {
            normal: { start: "#dddddd", end: "#f2f2f2", middle: "#dddddd" },
            active: { start: "#E28136", end: "#E28136", middle: "#E28136" }
        },
        checkShape: { image: {
            imageName: "checkShape.gif", imageCount: 2, imageWidth: 7, imageHeight: 7
        }
        },
        sortShape: { image: {
            imageName: "sortShape.gif", imageCount: 2, imageWidth: 7, imageHeight: 12
        }
        },
        rowDelete: { image: {
            imageName: "rowDelete.gif", imageCount: 2, imageWidth: 11, imageHeight: 11
        }
        },
        lockShape: { image: {
            imageName: "lock.gif", imageCount: 2, imageWidth: 9, imageHeight: 9
        }
        },
        maxShape: { image: { imageName: "max.gif", imageCount: 2, imageWidth: 18, imageHeight: 16} }
    },

    pager: {
        properties: {
            hoverCssClass: "PagerPageNumberHover"
        },
        prevPage: { image: {
            imageName: "prevPage.gif", imageCount: 4, imageWidth: 20, imageHeight: 18
        }
        },
        nextPage: { image: {
            imageName: "nextPage.gif", imageCount: 4, imageWidth: 20, imageHeight: 18
        }
        },
        refresh: { image: {
            imageName: "refresh.gif", imageCount: 4, imageWidth: 20, imageHeight: 18
        }
        }
    },

    menu: {
        properties: {
            iconWidth: 22, iconHeight: 22
        },
        menuRootActive: {
            image: {
                imageName: "menuRootActiveBg.gif", imageCount: 2, imageWidth: 1, imageHeight: 30
            },
            border: "0px solid #E1F2FD"
        },
        menuItemActive: {
            image: {
                imageName: "menuItemActiveBg.gif", imageCount: 2, imageWidth: 1, imageHeight: 28
            },
            borderColor: "#e3e3e3",
            imageMargin: "2px 4px 2px 2px",
            paddingRight: "5px"
        }
    },

    tab: {
        properties: { mdiSkin: "mdiTab" },
        colors: {
            normalText: "#ffffff", hoverText: "#ffffff", selectedText: ""
        },
        topTab: { image: {
            imageName: "topTab.gif", imageCount: 9, imageWidth: 10, imageHeight: 30
        }
        },
        bottomTab: { image: {
            imageName: "bottomTab.gif", imageCount: 9, imageWidth: 5, imageHeight: 20
        }
        },
        closeButton: { image: {
            imageName: "closeButton.gif", imageCount: 3, imageWidth: 13, imageHeight: 13
        }
        }
    },

    mdiTab: {
        colors: {
            normalText: "#333", hoverText: "#fff", selectedText: "#fff"
        },
        topTab: { image: {
            imageName: "topTab.gif", imageCount: 9, imageWidth: 9, imageHeight: 41
        }
        },
        bottomTab: { image: {
            imageName: "bottomTab.gif", imageCount: 9, imageWidth: 9, imageHeight: 41
        }
        },
        backTopShape: { image: {
            imageName: "backTop.gif", imageCount: 2, imageWidth: 52, imageHeight: 52
        }
        }
    },

    tree: {
        properties: { imageWidth: 16, imageHeight: 22 },

        minus: { image: { imageName: "minus.gif"} },
        minusLine: { image: { imageName: "minusLine.gif"} },

        plus: { image: { imageName: "plus.gif"} },
        plusLine: { image: { imageName: "plusLine.gif"} },

        none: { image: { imageName: "none.gif"} },
        noneLine: { image: { imageName: "noneLine.gif"} },

        checkShape: { image: {
            imageName: "checkShape.gif", imageCount: 3, imageWidth: 16, imageHeight: 22
        }
        },

        line: { image: { imageName: "line.gif"} },

        addButton: { image: { imageName: "add.gif", imageCount: 4, imageWidth: 18, imageHeight: 18 }
        },

        delButton: { image: { imageName: "del.gif", imageCount: 4, imageWidth: 18, imageHeight: 18 }
        }
    },

    menuTree: {
        properties: { imageWidth: 16, imageHeight: 22 },
        checkShape: {
            image: { imageName: "checkShape.gif", imageCount: 3, imageWidth: 16, imageHeight: 22 }
        },

        colors: {
            selectedBg: "#e1ebff", hoverBg: "#fdfcf4", nodeItemHoverBg: "#d3e5fa",
            textColor: "#000", hoverText: "#000", highlightText: "#000", nodeItemHoverText: "#000",
            curNormalBorderStyle: "2px solid #f7f5f5", curHoverBorderStyle: "2px solid #724497"
        }
    },

    toolBar: {
        buttonBg: { image: {
            imageName: "buttonBg.gif", imageCount: 3, imageWidth: 22, imageHeight: 20
        }
        },
        largeButtonBg: { image: {
            imageName: "largeButtonBg.gif", imageCount: 2, imageWidth: 1, imageHeight: 41
        }
        }
    },

    navBar: {
        itemActive: { bgColor: "#FDFCF4", border: "1px solid #B0DAEB" },
        headerBg: { image: {
            imageName: "headerBg.gif", imageCount: 2, imageWidth: 1, imageHeight: 32
        }
        },
        collapseExpand: { image: {
            imageName: "collapseExpand.gif", imageCount: 2, imageWidth: 16, imageHeight: 16
        }
        }
    }
}
  