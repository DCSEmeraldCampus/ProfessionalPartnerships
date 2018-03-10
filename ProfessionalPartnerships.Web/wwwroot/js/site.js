//helper for working with React
/* example
    return R("table", { className: "table" }, [
                Rs("thead",
                    R("tr", ui.cols.map(function (col, i) { return R("td", {key: i}, col.display) }))),
                R("tbody",makeRows())
*/
var R = function (node, propsOrChildren, children) {
    var args = [node];
    //if all arguments were supplied, let's just pass it through
    if (typeof (node) == "string") {
        if (arguments.length == 3) {
            args = arguments
        }
        else if (propsOrChildren && typeof (propsOrChildren) == "object" && !propsOrChildren.$$typeof && !(propsOrChildren instanceof Array)) {
            args.push(propsOrChildren);
            args.push(children);
        }
        //if propsOrChildren are supplied, but not a object then we'll assume it's the children
        else if (propsOrChildren) {
            args.push({});
            args.push(propsOrChildren);
        }
        else {
            args.push({});
            args.push(null);
        }
    } else {
        args = arguments;
    }
    // if props or children is an object and doesn't have the react $$typeof, then we'll assume it's a dict of attribbutes
    return React.createElement.apply(null, args);
}
var Rs = function (el) {
    return React.createElement.apply(null, [el, {}].concat(Array.prototype.slice.call(arguments, 1)))
}
