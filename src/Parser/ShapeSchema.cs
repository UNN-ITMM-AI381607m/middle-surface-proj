﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.6.1055.0.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class Shape2D {
    
    private Contour[] contourField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Contour")]
    public Contour[] Contour {
        get {
            return this.contourField;
        }
        set {
            this.contourField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class Contour {
    
    private ParserPoint[] jointsOfSegmentsField;
    
    private ParserPoint[][] segmentsField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("JunctionPoint", IsNullable=false)]
    public ParserPoint[] JointsOfSegments {
        get {
            return this.jointsOfSegmentsField;
        }
        set {
            this.jointsOfSegmentsField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("BezierCurve", IsNullable=false)]
    [System.Xml.Serialization.XmlArrayItemAttribute("ControlPoint", IsNullable=false, NestingLevel=1)]
    public ParserPoint[][] Segments {
        get {
            return this.segmentsField;
        }
        set {
            this.segmentsField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class ParserPoint {
    
    private double xField;
    
    private double yField;
    
    /// <remarks/>
    public double X {
        get {
            return this.xField;
        }
        set {
            this.xField = value;
        }
    }
    
    /// <remarks/>
    public double Y {
        get {
            return this.yField;
        }
        set {
            this.yField = value;
        }
    }
}
