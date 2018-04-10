var __skinData = {
    common: {
        colors: {
            controlText: "black", controlBg: "#ffffff", windowBg: "#ffffff",
            highlightBg: "#fff6e7", highlightText: "#ffa004",
            hoverBg: "#FDFCF4", hoverText: "#000000", activeBg: "#ffffff",
            disabledBg: "#fafafa", disabledText: "#9BBBE0", buttonHoverText: ""
        },
        buttonColors: { active: { start: "#ffa004", end: "#ffa004", middle: "#ffa004"} },
        buttonBg: { image: { imageName: "", imageCount: 1, imageWidth: 1, imageHeight: 23} },
        activeButtonBg: { image: { imageName: "", imageCount: 2, imageWidth: 1, imageHeight: 23} },
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
        pwdButton: { image: { imageName: "pwdButton.gif", imageCount: 2, imageWidth: 19, imageHeight: 12} },
        comboButton: { image: { imageName: "comboButton.png", imageCount: 4, imageWidth: 18, imageHeight: 20} },
        ellipsisButton: { image: { imageName: "ellipsisButton.png", imageCount: 4, imageWidth: 18, imageHeight: 20} },
        spinUp: { image: { imageName: "spinUp.png", imageCount: 4, imageWidth: 12, imageHeight: 12} },
        spinDown: { image: { imageName: "spinDown.png", imageCount: 4, imageWidth: 12, imageHeight: 12} },
        calcButton: { image: { imageName: "calcButton.png", imageCount: 4, imageWidth: 18, imageHeight: 20} }
    },

    form: {
        properties: { captionHeight: 22, smallCaptionHeight: 17 },
        captionBg: { image: { imageName: ""} },
        buttonClose: { image: { imageName: "buttonClose.png", imageCount: 4, imageWidth: 26, imageHeight: 24} },
        smallButtonClose: { image: { imageName: "smallButtonClose.png", imageCount: 4, imageWidth: 20, imageHeight: 18} }
    },

    grid: {
        properties: { borderSize:0, borderPadding: 1, rowNoColumnWidth: 40 },
        colors: {
            headerLine: "#B7B7B7;box-shadow:1px 2px 2px #fff",
            normalBg: "#ffffff", normalText: "#3f3f3f",
            evenBg: "#fafafa", evenText: "#3f3f3f", // Å¼ÊýÐÐ
            selectedBg: "#feda9e", selectedText: "#3f3f3f",
            hoverBg: "#edf4fe", hoverText: "#000000",
            disabledBg: "#F6FCFF", disabledText: "#aaaaaa"
        },
        headerColors: {
            normal: { start: "#f2f2f2", end: "#f2f2f2", middle: "#f2f2f2" },
            active: { start: "#ffa004", end: "#ffa004", middle: "#ffa004" }
        },
        checkShape: { image: { imageName: "checkShape.png", imageCount: 2, imageWidth: 12, imageHeight: 12} },
        sortShape: { image: { imageName: "sortShape.png", imageCount: 2, imageWidth: 8, imageHeight: 12} },
        rowDelete: { image: { imageName: "rowDelete.png", imageCount: 2, imageWidth: 16, imageHeight: 16} },
        lockShape: { image: { imageName: "lock.gif", imageCount: 2, imageWidth: 9, imageHeight: 9} },
        maxShape: { image: { imageName: "max.gif", imageCount: 2, imageWidth: 14, imageHeight: 14} }
    },

    pager: {
        properties: { hoverCssClass: "PagerPageNumberHover" },
        prevPage: { image: { imageName: "prevPage.png", imageCount: 4, imageWidth: 20, imageHeight: 12} },
        nextPage: { image: { imageName: "nextPage.png", imageCount: 4, imageWidth: 20, imageHeight: 12} },
        refresh: { image: { imageName: "refresh.png", imageCount: 4, imageWidth: 16, imageHeight: 20} }
    },

    menu: {
        properties: {
            iconWidth: 10, iconHeight: 10
        },
        menuRootActive: {
            image: { imageName: "", imageCount: 2, imageWidth: 1, imageHeight: 30 },
            border: "0px solid #E1F2FD"
        },
        menuItemActive: {
            image: { imageName: "", imageCount: 2, imageWidth: 1, imageHeight: 28 },
            borderColor: "",
            imageMargin: "",
            paddingRight: "5px"
        }
    },

    tab: {
        properties: { mdiSkin: "mdiTab" },
        colors: { normalText: "#fff", hoverText: "#fff", selectedText: "" },
        topTab: { image: { imageName: "", imageCount: 9, imageWidth: 2, imageHeight: 24} },
        bottomTab: { image: { imageName: "", imageCount: 9, imageWidth: 5, imageHeight: 20} },
        closeButton: { image: { imageName: "closeButton.png", imageCount: 3, imageWidth: 24, imageHeight: 24} }
    },

    mdiTab: {
        colors: { normalText: "#333", hoverText: "#fff", selectedText: "#fff" },
        topTab: { image: { imageName: "", imageCount: 9, imageWidth: 9, imageHeight: 41} },
        bottomTab: { image: { imageName: "", imageCount: 9, imageWidth: 9, imageHeight: 41} }
    },

    tree: {
        properties: { imageWidth: 18, imageHeight: 24 },

        minus: { image: { imageName: "minus.png"} },
        minusLine: { image: { imageName: "minus.png"} },

        plus: { image: { imageName: "plus.png"} },
        plusLine: { image: { imageName: "plus.png"} },

        none: { image: { imageName: "none.gif"} },
        noneLine: { image: { imageName: "none.gif"} },

        checkShape: { image: { imageName: "checkShape.png", imageCount: 3, imageWidth: 20, imageHeight: 24} },

        line: { image: { imageName: "none.gif"} },

        addButton: { image: { imageName: "add.png", imageCount: 4, imageWidth: 18, imageHeight: 18} },

        delButton: { image: { imageName: "del.png", imageCount: 4, imageWidth: 18, imageHeight: 18} }
    },

    menuTree: {
        properties: { imageWidth: 16, imageHeight: 24 },
        checkShape: { image: { imageName: "checkShape.png", imageCount: 3, imageWidth: 16, imageHeight: 24} },

        colors: {
            selectedBg: "#e1ebff", hoverBg: "#fdfcf4", nodeItemHoverBg: "#d3e5fa",
            textColor: "#000", hoverText: "#000", highlightText: "#000", nodeItemHoverText: "#000",
            curNormalBorderStyle: "2px solid #f7f5f5", curHoverBorderStyle: "2px solid #724497"
        }
    },

    toolBar: {
        buttonBg: { image: { imageName: "buttonBg.gif", imageCount: 3, imageWidth: 22, imageHeight: 20} },
        largeButtonBg: { image: { imageName: "largeButtonBg.gif", imageCount: 2, imageWidth: 1, imageHeight: 41} }
    },

    navBar: {
        itemActive: { bgColor: "#FDFCF4", border: "1px solid #B0DAEB" },
        headerBg: { image: { imageName: "headerBg.gif", imageCount: 2, imageWidth: 1, imageHeight: 32} },
        collapseExpand: { image: { imageName: "collapseExpand.png", imageCount: 2, imageWidth: 16, imageHeight: 16} }
    }
}
  