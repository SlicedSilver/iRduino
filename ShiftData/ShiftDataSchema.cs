using System.Xml.Serialization;

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true)]
public partial class ShiftData {
    
    private CarShiftStyle[] carShiftStylesField;
    
    private CarShiftRPMData[] carShiftRPMsField;
    
    /// <remarks/>
    public CarShiftStyle[] CarShiftStyles {
        get {
            return this.carShiftStylesField;
        }
        set {
            this.carShiftStylesField = value;
        }
    }
    
    /// <remarks/>
    public CarShiftRPMData[] CarShiftRPMs {
        get {
            return this.carShiftRPMsField;
        }
        set {
            this.carShiftRPMsField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true)]
public partial class CarShiftStyle {
    
    private int carIDField;
    
    private bool useCustomRPMSField;
    
    private string iRacingCarNameField;
    
    private CarShiftStyles shiftStyleField;
    
    /// <remarks/>
    public int CarID {
        get {
            return this.carIDField;
        }
        set {
            this.carIDField = value;
        }
    }
    
    /// <remarks/>
    public bool UseCustomRPMS {
        get {
            return this.useCustomRPMSField;
        }
        set {
            this.useCustomRPMSField = value;
        }
    }
    
    /// <remarks/>
    public string iRacingCarName {
        get {
            return this.iRacingCarNameField;
        }
        set {
            this.iRacingCarNameField = value;
        }
    }
    
    /// <remarks/>
    public CarShiftStyles ShiftStyle {
        get {
            return this.shiftStyleField;
        }
        set {
            this.shiftStyleField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public enum CarShiftStyles {
    
    /// <remarks/>
    LeftToRight,
    
    /// <remarks/>
    LeftToRight3Segments,
    
    /// <remarks/>
    RightToLeft,
    
    /// <remarks/>
    Converging,
    
    /// <remarks/>
    SingleLight,
    
    /// <remarks/>
    NoLights,

    None
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true)]
public partial class CarShiftRPMData {
    
    private int carIDField;
    
    private string mapVariableField;
    
    private ShiftMap[] mapsField;
    
    /// <remarks/>
    public int CarID {
        get {
            return this.carIDField;
        }
        set {
            this.carIDField = value;
        }
    }
    
    /// <remarks/>
    public string MapVariable {
        get {
            return this.mapVariableField;
        }
        set {
            this.mapVariableField = value;
        }
    }
    
    /// <remarks/>
    public ShiftMap[] Maps {
        get {
            return this.mapsField;
        }
        set {
            this.mapsField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true)]
public partial class ShiftMap {
    
    private int mapValueField;
    
    private ShiftRPMs[] gearsField;
    
    /// <remarks/>
    public int MapValue {
        get {
            return this.mapValueField;
        }
        set {
            this.mapValueField = value;
        }
    }
    
    /// <remarks/>
    public ShiftRPMs[] Gears {
        get {
            return this.gearsField;
        }
        set {
            this.gearsField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true)]
public partial class ShiftRPMs {
    
    private int gearField;
    
    private int firstField;
    
    private int lastField;
    
    private int shiftField;
    
    private int blinkField;
    
    /// <remarks/>
    public int Gear {
        get {
            return this.gearField;
        }
        set {
            this.gearField = value;
        }
    }
    
    /// <remarks/>
    public int First {
        get {
            return this.firstField;
        }
        set {
            this.firstField = value;
        }
    }
    
    /// <remarks/>
    public int Last {
        get {
            return this.lastField;
        }
        set {
            this.lastField = value;
        }
    }
    
    /// <remarks/>
    public int Shift {
        get {
            return this.shiftField;
        }
        set {
            this.shiftField = value;
        }
    }
    
    /// <remarks/>
    public int Blink {
        get {
            return this.blinkField;
        }
        set {
            this.blinkField = value;
        }
    }
}
