// helpers --------------------------------
function extend(Child, Parent) {
    var F = function () { }
    F.prototype = Parent.prototype
    Child.prototype = new F()
    Child.prototype.constructor = Child
    Child.superclass = Parent.prototype
}

function DilogUIHelper(doc, dataContainer) {
    this.$doc = doc;
    this.$dataContainer = dataContainer;
}

DilogUIHelper.prototype.showEmptyContainer = function (message) {
    var height = this.$doc.height() / 3;//calculate approx. height
    this.$dataContainer.empty();
    this.$dataContainer.html("<div style='height:" + height + "px;'><label style='top:40%; position: relative;'><h5><i>" + message + "</i></h5></label></div>");
}

//end helpers------------------------------