using System;

class MyPlayerClass : FQ.BaseBody {

    public string name;

    public MyPlayerClass (string name) {
        this.name = name;
    }

    public new string toJson () {
        return "{ name: " + this.name + ", _id: " + this._id + " }";
    }

}