NS = {
}
NS.Mammal = function (name) {
    this.name = name;
    this.offspring = [
    ];
}
NS.Mammal.prototype.haveABaby = function () {
    var newBaby = new NS.Mammal('Baby ' + this.name);
    this.offspring.push(newBaby);
    return newBaby;
}
NS.Mammal.prototype.toString = function () {
    return '[Mammal "' + this.name + '"]';
}
NS.Cat = function (name) {
    this.name = name;
}
NS.Cat.prototype = new NS.Mammal();
// Here's where the inheritance occurs
NS.Cat.prototype.constructor = NS.Cat;
// Otherwise instances of Cat would have a constructor of Mammal
NS.Cat.prototype.toString = function () {
    return '[Cat "' + this.name + '"]';
}
var someAnimal = new NS.Mammal('Mr. Biggles');
var myPet = new NS.Cat('Felix');
// results in '[Mammal "Baby Felix"]'
var EcmaSharp = EcmaSharp || {
};
var ClrObject = function () {
    this.GetHashCode = function () {
        return Object.keys(this);
    };
    return this;
};
EcmaSharp.SomeJSClass = function () {
    return function (value) {
        var _value = 0;
        var Text = null;
        _value = value;
        var self = new ClrObject();
        self.get_Text = function () {
            return Text;
        };
        self.set_Text = function (__value__) {
            Text = __value__;
        };
        self.Increment = function (count) {
            _value++;
            Text = '';
            for (var i = 0; i < _value; i++)
                Text += i.toString();
            return Text.length;
        };
        self.toString = function () {
            return self.get_Text();
        };
        return self;
    };
}();
var EcmaSharp = EcmaSharp || {
};
var sjs = new EcmaSharp.SomeJSClass(14);
sjs.Increment(5);
sjs.GetHashCode();
sjs.get_Text();
sjs.toString();
var vanillaStyle2 = {
};


vanillaStyle2.Class1 = function () {
    this._start = 0,
    this.depth = function () {
        return this._start;
    },
    this.getHashCode = function () { return this.Text; }
};
vanillaStyle2.Class2 = function () {
    this._super = new vanillaStyle2.Class1();
    this.depth = function () {
        return this._super.depth() + 1;
    };
    this.getHashCode = this._super.getHashCode;
}
vanillaStyle2.Class3 = function () {
    this._super = new vanillaStyle2.Class2();
    this.depth = function () {
        return this._super.depth() + 1;
    }
    this.getHashCode = this._super.getHashCode;
    this.Text = 'asdf';
}
var vanilla2 = new vanillaStyle2.Class3();
vanilla2.depth();
vanilla2.getHashCode();



var vanillaStyle = {};
vanillaStyle.Class1 = function () { };
vanillaStyle.Class1.prototype = {
    _start: 0,
    depth: function () {
        return this._start;
    }
};

vanillaStyle.Class2 = function () { };
vanillaStyle.Class2.prototype = new vanillaStyle.Class1();
vanillaStyle.Class2.depth = function () {
    return vanillaStyle.Class1.prototype.depth.call(this) + 1;
};

vanillaStyle.Class3 = function () { };
vanillaStyle.Class3.prototype = new vanillaStyle.Class2();
vanillaStyle.Class3.prototype.depth = function () {
    return vanillaStyle.Class2.prototype.depth.call(this) + 1;
};

var vanilla = new vanillaStyle.Class3();
vanilla.depth();