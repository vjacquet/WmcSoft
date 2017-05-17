using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.CustomMarshalers;
using System.Security;
using System.Collections;

namespace WmcSoft.TextObjectModel
{
    [InterfaceType(ComInterfaceType.InterfaceIsDual),
    ComVisible(true),
    SuppressUnmanagedCodeSecurity,
    Guid("8CC497C0-A1DF-11CE-8098-00AA0047BE5D"),
    DefaultMember("Name")]
    public interface ITextDocument
    {
        [DispId(0)]
        string Name {
            [return: MarshalAs(UnmanagedType.BStr)]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
            get;
        }
        [DispId(1)]
        ITextSelection Selection {
            [return: MarshalAs(UnmanagedType.Interface)]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)]
            get;
        }
        [DispId(2)]
        int StoryCount {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
            get;
        }
        [DispId(3)]
        ITextStoryRanges StoryRanges {
            [return: MarshalAs(UnmanagedType.Interface)]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)]
            get;
        }
        [DispId(4)]
        int Saved {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)]
            set;
        }
        [DispId(5)]
        float DefaultTabStop {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)]
            set;
        }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(6)]
        void New();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(7)]
        void Open([In, MarshalAs(UnmanagedType.Struct)] ref object pVar, [In] int Flags, [In] int CodePage);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(8)]
        void Save([In, MarshalAs(UnmanagedType.Struct)] ref object pVar, [In] int Flags, [In] int CodePage);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(9)]
        int Freeze();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(10)]
        int Unfreeze();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(11)]
        void BeginEditCollection();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(12)]
        void EndEditCollection();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(13)]
        int Undo([In] int Count);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(14)]
        int Redo([In] int Count);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(15)]
        ITextRange Range([In] int cp1, [In] int cp2);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x10)]
        ITextRange RangeFromPoint([In] int x, [In] int y);
    }

    [ComImport, Guid("8CC497C3-A1DF-11CE-8098-00AA0047BE5D"), DefaultMember("Duplicate"), TypeLibType(0xc0)]
    public interface ITextFont
    {
        [DispId(0)]
        ITextFont Duplicate {
            [return: MarshalAs(UnmanagedType.Interface)]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
            get;
            [param: In, MarshalAs(UnmanagedType.Interface)]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
            set;
        }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x301)]
        int CanChange();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(770)]
        int IsEqual([In, MarshalAs(UnmanagedType.Interface)] ITextFont pFont);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x303)]
        void Reset([In] int Value);
        [DispId(0x304)]
        int Style {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x304)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x304)]
            set;
        }
        [DispId(0x305)]
        int AllCaps {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x305)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x305)]
            set;
        }
        [DispId(0x306)]
        int Animation {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x306)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x306)]
            set;
        }
        [DispId(0x307)]
        int BackColor {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x307)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x307)]
            set;
        }
        [DispId(0x308)]
        int Bold {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x308)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x308)]
            set;
        }
        [DispId(0x309)]
        int Emboss {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x309)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x309)]
            set;
        }
        [DispId(0x310)]
        int ForeColor {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x310)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x310)]
            set;
        }
        [DispId(0x311)]
        int Hidden {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x311)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x311)]
            set;
        }
        [DispId(0x312)]
        int Engrave {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x312)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x312)]
            set;
        }
        [DispId(0x313)]
        int Italic {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x313)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x313)]
            set;
        }
        [DispId(0x314)]
        float Kerning {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x314)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x314)]
            set;
        }
        [DispId(0x315)]
        int LanguageID {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x315)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x315)]
            set;
        }
        [DispId(790)]
        string Name {
            [return: MarshalAs(UnmanagedType.BStr)]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(790)]
            get;
            [param: In, MarshalAs(UnmanagedType.BStr)]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(790)]
            set;
        }
        [DispId(0x317)]
        int Outline {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x317)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x317)]
            set;
        }
        [DispId(0x318)]
        float Position {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x318)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x318)]
            set;
        }
        [DispId(0x319)]
        int Protected {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x319)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x319)]
            set;
        }
        [DispId(800)]
        int Shadow {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(800)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(800)]
            set;
        }
        [DispId(0x321)]
        float Size {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x321)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x321)]
            set;
        }
        [DispId(0x322)]
        int SmallCaps {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x322)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x322)]
            set;
        }
        [DispId(0x323)]
        float Spacing {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x323)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x323)]
            set;
        }
        [DispId(0x324)]
        int StrikeThrough {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x324)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x324)]
            set;
        }
        [DispId(0x325)]
        int Subscript {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x325)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x325)]
            set;
        }
        [DispId(0x326)]
        int Superscript {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x326)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x326)]
            set;
        }
        [DispId(0x327)]
        int Underline {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x327)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x327)]
            set;
        }
        [DispId(0x328)]
        int Weight {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x328)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x328)]
            set;
        }
    }

    [ComImport, Guid("8CC497C4-A1DF-11CE-8098-00AA0047BE5D"), TypeLibType(0xc0), DefaultMember("Duplicate")]
    public interface ITextPara
    {
        [DispId(0)]
        ITextPara Duplicate {
            [return: MarshalAs(UnmanagedType.Interface)]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
            get;
            [param: In, MarshalAs(UnmanagedType.Interface)]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
            set;
        }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x401)]
        int CanChange();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x402)]
        int IsEqual([In, MarshalAs(UnmanagedType.Interface)] ITextPara pPara);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x403)]
        void Reset([In] int Value);
        [DispId(0x404)]
        int Style {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x404)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x404)]
            set;
        }
        [DispId(0x405)]
        int Alignment {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x405)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x405)]
            set;
        }
        [DispId(0x406)]
        int Hyphenation {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x406)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x406)]
            set;
        }
        [DispId(0x407)]
        float FirstLineIndent {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x407)]
            get;
        }
        [DispId(0x408)]
        int KeepTogether {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x408)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x408)]
            set;
        }
        [DispId(0x409)]
        int KeepWithNext {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x409)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x409)]
            set;
        }
        [DispId(0x410)]
        float LeftIndent {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x410)]
            get;
        }
        [DispId(0x411)]
        float LineSpacing {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x411)]
            get;
        }
        [DispId(0x412)]
        int LineSpacingRule {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x412)]
            get;
        }
        [DispId(0x413)]
        int ListAlignment {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x413)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x413)]
            set;
        }
        [DispId(0x414)]
        int ListLevelIndex {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x414)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x414)]
            set;
        }
        [DispId(0x415)]
        int ListStart {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x415)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x415)]
            set;
        }
        [DispId(0x416)]
        float ListTab {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x416)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x416)]
            set;
        }
        [DispId(0x417)]
        int ListType {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x417)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x417)]
            set;
        }
        [DispId(0x418)]
        int NoLineNumber {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x418)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x418)]
            set;
        }
        [DispId(0x419)]
        int PageBreakBefore {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x419)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x419)]
            set;
        }
        [DispId(0x420)]
        float RightIndent {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x420)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x420)]
            set;
        }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x421)]
        void SetIndents([In] float StartIndent, [In] float LeftIndent, [In] float RightIndent);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x422)]
        void SetLineSpacing([In] int LineSpacingRule, [In] float LineSpacing);
        [DispId(0x423)]
        float SpaceAfter {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x423)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x423)]
            set;
        }
        [DispId(0x424)]
        float SpaceBefore {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x424)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x424)]
            set;
        }
        [DispId(0x425)]
        int WidowControl {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x425)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x425)]
            set;
        }
        [DispId(0x426)]
        int TabCount {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x426)]
            get;
        }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x427)]
        void AddTab([In] float tbPos, [In] int tbAlign, [In] int tbLeader);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x428)]
        void ClearAllTabs();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x429)]
        void DeleteTab([In] float tbPos);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x430)]
        void GetTab([In] int iTab, [Out] out float ptbPos, [Out] out int ptbAlign, [Out] out int ptbLeader);
    }

    [ComImport, TypeLibType(0xc0), Guid("8CC497C2-A1DF-11CE-8098-00AA0047BE5D"), DefaultMember("Text")]
    public interface ITextRange
    {
        [DispId(0)]
        string Text {
            [return: MarshalAs(UnmanagedType.BStr)]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
            get;
            [param: In, MarshalAs(UnmanagedType.BStr)]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
            set;
        }
        [DispId(0x201)]
        int Char {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x201)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x201)]
            set;
        }
        [DispId(0x202)]
        ITextRange Duplicate {
            [return: MarshalAs(UnmanagedType.Interface)]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x202)]
            get;
        }
        [DispId(0x203)]
        ITextRange FormattedText {
            [return: MarshalAs(UnmanagedType.Interface)]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x203)]
            get;
            [param: In, MarshalAs(UnmanagedType.Interface)]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x203)]
            set;
        }
        [DispId(0x204)]
        int Start {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x204)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x204)]
            set;
        }
        [DispId(0x205)]
        int End {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x205)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x205)]
            set;
        }
        [DispId(0x206)]
        ITextFont Font {
            [return: MarshalAs(UnmanagedType.Interface)]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x206)]
            get;
            [param: In, MarshalAs(UnmanagedType.Interface)]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x206)]
            set;
        }
        [DispId(0x207)]
        ITextPara Para {
            [return: MarshalAs(UnmanagedType.Interface)]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x207)]
            get;
            [param: In, MarshalAs(UnmanagedType.Interface)]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x207)]
            set;
        }
        [DispId(520)]
        int StoryLength {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(520)]
            get;
        }
        [DispId(0x209)]
        int StoryType {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x209)]
            get;
        }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x210)]
        void Collapse([In] int bStart);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x211)]
        int Expand([In] int Unit);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(530)]
        int GetIndex([In] int Unit);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x213)]
        void SetIndex([In] int Unit, [In] int Index, [In] int Extend);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x214)]
        void SetRange([In] int cpActive, [In] int cpOther);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x215)]
        int InRange([In, MarshalAs(UnmanagedType.Interface)] ITextRange pRange);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x216)]
        int InStory([In, MarshalAs(UnmanagedType.Interface)] ITextRange pRange);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x217)]
        int IsEqual([In, MarshalAs(UnmanagedType.Interface)] ITextRange pRange);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x218)]
        void Select();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x219)]
        int StartOf([In] int Unit, [In] int Extend);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x220)]
        int EndOf([In] int Unit, [In] int Extend);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x221)]
        int Move([In] int Unit, [In] int Count);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x222)]
        int MoveStart([In] int Unit, [In] int Count);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x223)]
        int MoveEnd([In] int Unit, [In] int Count);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x224)]
        int MoveWhile([In, MarshalAs(UnmanagedType.Struct)] ref object Cset, [In] int Count);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x225)]
        int MoveStartWhile([In, MarshalAs(UnmanagedType.Struct)] ref object Cset, [In] int Count);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(550)]
        int MoveEndWhile([In, MarshalAs(UnmanagedType.Struct)] ref object Cset, [In] int Count);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x227)]
        int MoveUntil([In, MarshalAs(UnmanagedType.Struct)] ref object Cset, [In] int Count);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x228)]
        int MoveStartUntil([In, MarshalAs(UnmanagedType.Struct)] ref object Cset, [In] int Count);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x229)]
        int MoveEndUntil([In, MarshalAs(UnmanagedType.Struct)] ref object Cset, [In] int Count);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(560)]
        int FindText([In, MarshalAs(UnmanagedType.BStr)] string bstr, [In] int cch, [In] int Flags);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x231)]
        int FindTextStart([In, MarshalAs(UnmanagedType.BStr)] string bstr, [In] int cch, [In] int Flags);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x232)]
        int FindTextEnd([In, MarshalAs(UnmanagedType.BStr)] string bstr, [In] int cch, [In] int Flags);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x233)]
        int Delete([In] int Unit, [In] int Count);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x234)]
        void Cut([Out, MarshalAs(UnmanagedType.Struct)] out object pVar);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x235)]
        void Copy([Out, MarshalAs(UnmanagedType.Struct)] out object pVar);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x236)]
        void Paste([In, MarshalAs(UnmanagedType.Struct)] ref object pVar, [In] int Format);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x237)]
        int CanPaste([In, MarshalAs(UnmanagedType.Struct)] ref object pVar, [In] int Format);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x238)]
        int CanEdit();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x239)]
        void ChangeCase([In] int Type);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x240)]
        void GetPoint([In] int Type, [Out] out int px, [Out] out int py);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x241)]
        void SetPoint([In] int x, [In] int y, [In] int Type, [In] int Extend);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x242)]
        void ScrollIntoView([In] int Value);
        [return: MarshalAs(UnmanagedType.IUnknown)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x243)]
        object GetEmbeddedObject();
    }

    [ComImport, Guid("8CC497C1-A1DF-11CE-8098-00AA0047BE5D"), TypeLibType(0xc0), DefaultMember("Text")]
    public interface ITextSelection : ITextRange
    {
        [DispId(0x101)]
        int Flags {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x101)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x101)]
            set;
        }
        [DispId(0x102)]
        int Type {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x102)]
            get;
        }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x103)]
        int MoveLeft([In] int Unit, [In] int Count, [In] int Extend);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(260)]
        int MoveRight([In] int Unit, [In] int Count, [In] int Extend);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x105)]
        int MoveUp([In] int Unit, [In] int Count, [In] int Extend);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x106)]
        int MoveDown([In] int Unit, [In] int Count, [In] int Extend);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x107)]
        int HomeKey([In] int Unit, [In] int Extend);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x108)]
        int EndKey([In] int Unit, [In] int Extend);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x109)]
        void TypeText([In, MarshalAs(UnmanagedType.BStr)] string bstr);
    }

    [ComImport, Guid("8CC497C5-A1DF-11CE-8098-00AA0047BE5D"), DefaultMember("Item"), TypeLibType(0xc0)]
    public interface ITextStoryRanges : IEnumerable
    {
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "", MarshalTypeRef = typeof(EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc(1), DispId(-4)]
        new IEnumerator GetEnumerator();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
        ITextRange Item([In] int Index);
        [DispId(2)]
        int Count {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
            get;
        }
    }

    public enum Constants
    {
        // Fields
        AlignBar = 4,
        AlignCenter = 1,
        AlignDecimal = 3,
        AlignJustify = 3,
        AlignLeft = 0,
        AlignRight = 2,
        AnimationMax = 8,
        AutoColor = -9999997,
        Backward = -1073741823,
        BlinkingBackground = 2,
        Cell = 12,
        Character = 1,
        CharFormat = 13,
        CollapseEnd = 0,
        CollapseStart = 1,
        Column = 9,
        CommentsStory = 4,
        CreateAlways = 0x20,
        CreateNew = 0x10,
        Dashes = 2,
        Default = -9999996,
        Dots = 1,
        Dotted = 4,
        Double = 3,
        End = 0,
        EndnotesStory = 3,
        EvenPagesFooterStory = 8,
        EvenPagesHeaderStory = 6,
        Extend = 1,
        False = 0,
        FirstPageFooterStory = 11,
        FirstPageHeaderStory = 10,
        FootnotesStory = 2,
        Forward = 0x3fffffff,
        HTML = 3,
        LasVegasLights = 1,
        Line = 5,
        Lines = 3,
        LineSpace1pt5 = 1,
        LineSpaceAtLeast = 3,
        LineSpaceDouble = 2,
        LineSpaceExactly = 4,
        LineSpaceMultiple = 5,
        LineSpaceSingle = 0,
        ListBullet = 1,
        ListNone = 0,
        ListNumberAsArabic = 2,
        ListNumberAsLCLetter = 3,
        ListNumberAsLCRoman = 5,
        ListNumberAsSequence = 7,
        ListNumberAsUCLetter = 4,
        ListNumberAsUCRoman = 6,
        ListParentheses = 0x10000,
        ListPeriod = 0x20000,
        ListPlain = 0x30000,
        LowerCase = 0,
        MainTextStory = 1,
        MarchingBlackAnts = 4,
        MarchingRedAnts = 5,
        MatchCase = 4,
        MatchPattern = 8,
        MatchWord = 2,
        Move = 0,
        NoAnimation = 0,
        None = 0,
        NoSelection = 0,
        Object = 0x10,
        OpenAlways = 0x40,
        OpenExisting = 0x30,
        ParaFormat = 14,
        Paragraph = 4,
        PasteFile = 0x1000,
        PrimaryFooterStory = 9,
        PrimaryHeaderStory = 7,
        ReadOnly = 0x100,
        Row = 10,
        RTF = 1,
        Screen = 7,
        Section = 8,
        SelActive = 8,
        SelAtEOL = 2,
        SelectionBlock = 6,
        SelectionColumn = 4,
        SelectionFrame = 3,
        SelectionInlineShape = 7,
        SelectionIP = 1,
        SelectionNormal = 2,
        SelectionRow = 5,
        SelectionShape = 8,
        SelOvertype = 4,
        SelReplace = 0x10,
        SelStartActive = 1,
        Sentence = 3,
        SentenceCase = 4,
        ShareDenyRead = 0x200,
        ShareDenyWrite = 0x400,
        Shimmer = 6,
        Single = 1,
        Spaces = 0,
        SparkleText = 3,
        Start = 0x20,
        Story = 6,
        TabBack = -3,
        TabHere = -1,
        Table = 15,
        TabNext = -2,
        Text = 2,
        TextFrameStory = 5,
        TitleCase = 2,
        Toggle = -9999998,
        ToggleCase = 5,
        True = -1,
        TruncateExisting = 80,
        Undefined = -9999999,
        UnknownStory = 0,
        UpperCase = 1,
        Window = 11,
        WipeDown = 7,
        WipeRight = 8,
        Word = 2,
        WordDocument = 4,
        Words = 2
    }

#if C
	[InterfaceType(ComInterfaceType.InterfaceIsDual), SuppressUnmanagedCodeSecurity, Guid("8CC497C0-A1DF-11ce-8098-00AA0047BE5D"), ComVisible(true)]
	public interface ITextDocument
	{
		string GetName();
		object GetSelection();
		int GetStoryCount();
		object GetStoryRanges();
		int GetSaved();
		void SetSaved(int value);
		object GetDefaultTabStop();
		void SetDefaultTabStop(object value);
		void New();
		void Open(object pVar, int flags, int codePage);
		void Save(object pVar, int flags, int codePage);
		int Freeze();
		int Unfreeze();
		void BeginEditCollection();
		void EndEditCollection();
		int Undo(int count);
		int Redo(int count);
		[return: MarshalAs(UnmanagedType.Interface)]
		ITextRange Range(int cp1, int cp2);
		[return: MarshalAs(UnmanagedType.Interface)]
		ITextRange RangeFromPoint(int x, int y);
	}

	[InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true), SuppressUnmanagedCodeSecurity, Guid("8CC497C2-A1DF-11ce-8098-00AA0047BE5D")]
	public interface ITextRange
	{
		string GetText();
		void SetText(string text);
		object GetChar();
		void SetChar(object ch);
		[return: MarshalAs(UnmanagedType.Interface)]
		ITextRange GetDuplicate();
		[return: MarshalAs(UnmanagedType.Interface)]
		ITextRange GetFormattedText();
		void SetFormattedText([In, MarshalAs(UnmanagedType.Interface)] ITextRange range);
		int GetStart();
		void SetStart(int cpFirst);
		int GetEnd();
		void SetEnd(int cpLim);
		object GetFont();
		void SetFont(object font);
		object GetPara();
		void SetPara(object para);
		int GetStoryLength();
		int GetStoryType();
		void Collapse(int start);
		int Expand(int unit);
		int GetIndex(int unit);
		void SetIndex(int unit, int index, int extend);
		void SetRange(int cpActive, int cpOther);
		int InRange([In, MarshalAs(UnmanagedType.Interface)] ITextRange range);
		int InStory([In, MarshalAs(UnmanagedType.Interface)] ITextRange range);
		int IsEqual([In, MarshalAs(UnmanagedType.Interface)] ITextRange range);
		void Select();
		int StartOf(int unit, int extend);
		int EndOf(int unit, int extend);
		int Move(int unit, int count);
		int MoveStart(int unit, int count);
		int MoveEnd(int unit, int count);
		int MoveWhile(object cset, int count);
		int MoveStartWhile(object cset, int count);
		int MoveEndWhile(object cset, int count);
		int MoveUntil(object cset, int count);
		int MoveStartUntil(object cset, int count);
		int MoveEndUntil(object cset, int count);
		int FindText(string text, int cch, int flags);
		int FindTextStart(string text, int cch, int flags);
		int FindTextEnd(string text, int cch, int flags);
		int Delete(int unit, int count);
		void Cut(out object pVar);
		void Copy(out object pVar);
		void Paste(object pVar, int format);
		int CanPaste(object pVar, int format);
		int CanEdit();
		void ChangeCase(int type);
		void GetPoint(int type, out int x, out int y);
		void SetPoint(int x, int y, int type, int extend);
		void ScrollIntoView(int value);
		object GetEmbeddedObject();
	}
 
#endif
}
