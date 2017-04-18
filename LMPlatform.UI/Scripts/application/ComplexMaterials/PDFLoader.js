PDFJS.workerSrc = '/Scripts/pdfjs/pdf.worker.js';
// pdf ---------------------

extend(PDFLoader, BaseLoader)

function PDFLoader(dataContainer, loader, doc, dialog, timer) {
    BaseLoader.call(this, dataContainer, loader, doc, dialog);

    var scale = 4.5;
    var scaleStep = 0.1;
    var scope = this;
    this.thePDF = null;

    this.scalingViewport = function (page, scale, maxWidth) {
        var viewPort = page.getViewport(scale);
        if ((viewPort.width + viewPort.width * 0.1) < maxWidth)
            return viewPort;
        else
            return scope.scalingViewport(page, scale - scaleStep, maxWidth);
    }

    this.renderPdf = function () {
        timer.startTimer();
        currPage = 1;
        this.clearDataContainer();
        this.thePDF.getPage(currPage).then(this.handlePages);
    }

    this.handlePages = function (page) {

        var canvas = document.createElement("canvas");
        var viewport = scope.scalingViewport(page, scale, $('.modal-dialog').width());
        scale = viewport.scale;
        canvas.height = viewport.height;
        canvas.width = viewport.width;
        canvas.className = "pdf-page";

        var context = canvas.getContext('2d');
        page.render({ canvasContext: context, viewport: viewport });

        scope.$dataContainer.append(canvas);

        currPage++;
        if (scope.thePDF !== null && currPage <= numPages) {
            scope.thePDF.getPage(currPage).then(scope.handlePages, scope);
        }
        else
            scope.stopSpin();
    }

    this.zoomPlus = function () {
        this.clearDataContainer();
        currPage = 1;
        scale = scale + scaleStep;
        this.thePDF.getPage(currPage).then(this.handlePages);
    }

    this.zoomMinus = function () {
        this.clearDataContainer();
        currPage = 1;
        scale = scale - scaleStep;
        this.thePDF.getPage(currPage).then(this.handlePages);
    }


}

PDFLoader.prototype.loadData = function (filePath) {
    var scope = this;
    this.startSpin();
    this.clearDataContainer();
    PDFJS.getDocument(filePath).then(function (pdf) {
        scope.thePDF = pdf;
        numPages = pdf.numPages;
        scope.renderPdf();
    }).catch(function (error) {
        scope.stopSpin();
        if (error.name === 'InvalidPDFException') {
            scope.showEmptyContainer("Не поддерживаемый формат документа. Наиболее вероятной причиной подобной ошибки является загруженный документ не в формате PDF. Убедитесь, что Вы загрузили документ в формате PDF и повторите попытку открытия документа");
        }
        else {
            scope.showEmptyContainer("Произошла ошибка при загрузке объекта. Обратитесь к Адмнистратору");
        }
        alertify.error("Произошла ошибка при открытии объекта");
    });;
}

// end pdf ---------------------


