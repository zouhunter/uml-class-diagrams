//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Events;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine.TestTools;
//using NUnit.Framework;
//using ICSharpCode;
//using ICSharpCode.NRefactory;
//using ICSharpCode.NRefactory.Ast;
//using ICSharpCode.NRefactory.AstBuilder;
//using ICSharpCode.NRefactory.Parser;
//using ICSharpCode.NRefactory.Parser.CSharp;
//using ICSharpCode.NRefactory.PrettyPrinter;
//using ICSharpCode.NRefactory.Visitors;
//using System.Linq;
//using System.IO;
//using System.Text;
//using System.CodeDom;
//using Microsoft.CSharp;
//using System.CodeDom.Compiler;

////public class CodeDomCodeProvider : CodeDomProvider
////{
////    public CodeDomCodeProvider()
////            : base()
////    {
////    }

////    public override string FileExtension
////    {
////        get
////        {
////            return "cs";
////        }
////    }

////    public override System.CodeDom.Compiler.ICodeGenerator CreateGenerator()
////    {
////        return new CodeGenerator();
////    }

////    public override System.CodeDom.Compiler.ICodeCompiler CreateCompiler()
////    {
////        return null;
////    }
////}

//public class UmlTest
//{
//    private string classStr = @"
//    public class DemoClass
//    {
//        public int DemoInt;
//        public DemoClass()
//        {
//            DemoInt = 2;
//        }
//        public void DemoFunction(int arg)
//        {
//            DemoInt = arg;
//            Debug.Log(" + "\"Hellow World\"" + @");
//        }
//    }
//";
//    private void RePrintClass()
//    {
//        using (var reader = new StringReader(classStr))
//        {
//            Debug.Log(Generate(SupportedLanguage.CSharp, reader));
//        }
//    }
//    string Generate(ICSharpCode.NRefactory.SupportedLanguage language, TextReader inputstream)
//    {
//        ICSharpCode.NRefactory.IParser parser = ParserFactory.CreateParser(
//            language, inputstream);
//        parser.Parse();
//        Microsoft.CSharp.CSharpCodeProvider provider = new CSharpCodeProvider();

//        CodeDomVisitor visit = new CodeDomVisitor();

//        visit.VisitCompilationUnit(parser.CompilationUnit, null);

//        // Remove Unsed Namespaces
//        for (int i = visit.codeCompileUnit.Namespaces.Count - 1; i >= 0; i--)
//        {
//            if (visit.codeCompileUnit.Namespaces[i].Types.Count == 0)
//            {
//                visit.codeCompileUnit.Namespaces.RemoveAt(i);
//            }
//        }

//        CodeGeneratorOptions option = new CodeGeneratorOptions();
//        option.BlankLinesBetweenMembers = true;

//        using (StringWriter sw = new System.IO.StringWriter())
//        {
//            provider.GenerateCodeFromCompileUnit(
//          visit.codeCompileUnit, sw, option);
//            return sw.ToString();
//        }
//    }

//    [Test]
//    public void TestPrease()
//    {
//        RePrintClass();
//    }

//}
////Roles

////Role

////Role<T>

////UsingDeclaration:AbstractNode

////MemberType: AstType : AbstractNode

////AstType : AbstractNode

////C# Syntax Tree 	Unresolved Type System	Resolved Type System
////AstType ITypeReference IType
////TypeDeclaration IUnresolvedTypeDefinition   ITypeDefinition
////EntityDeclaration   IUnresolvedEntity IEntity
////FieldDeclaration IUnresolvedField    IField
////PropertyDeclaration / IndexerDeclaration IUnresolvedProperty IProperty
////MethodDeclaration / ConstructorDeclaration / OperatorDeclaration /
////Accessor IUnresolvedMethod   IMethod
////EventDeclaration    IUnresolvedEvent IEvent
////Attribute IUnresolvedAttribute    IAttribute
////Expression  IConstantValue ResolveResult
////PrivateImplementationType IMemberReference    IMember
////ParameterDeclaration    IUnresolvedParameter IParameter
////Accessor IUnresolvedMethod   IMethod
////NamespaceDeclaration    UsingScope ResolvedUsingScope
////-	-	INamespace
////SyntaxTree  IUnresolvedFile 	-

////- 	IUnresolvedAssembly IAssembly
////- 	IProjectContent ICompilation

////var document = new ICSharpCode.NRefactory.Editor.StringBuilderDocument("");
////var formattingOptions = FormattingOptionsFactory.CreateAllman();
////var options = new TextEditorOptions();
////using (var script = new DocumentScript(document, formattingOptions, options))
////{
////    script.InsertText(0, root.GetText());
////}
////Debug.Log(document.Text);
////SaveToAssetFolder(document.Text);

