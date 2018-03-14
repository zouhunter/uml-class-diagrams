using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

using ICSharpCode;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Documentation;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.PatternMatching;
using ICSharpCode.NRefactory.Utils;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;

using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.CSharp.Refactoring;
using ICSharpCode.NRefactory.CSharp.Analysis;
using ICSharpCode.NRefactory.CSharp.TypeSystem;
using ICSharpCode.NRefactory.CSharp.Completion;

using UnityEngine.Experimental.UIElements;
using System.Linq;
using System.IO;
using System.Text;

public class UmlTest
{
    private string classStr = @"
    public class DemoClass
    {
        public int DemoInt;
        public DemoClass()
        {
            DemoInt = 2;
        }
        public void DemoFunction(int arg)
        {
            DemoInt = arg;
            Debug.Log(" + "\"Hellow World\"" + @");
        }
    }
";
    private AstNode GetClassNode()
    {
        CompilerSettings setting = new CompilerSettings();
        CSharpParser cpaser = new CSharpParser(setting);
        ICSharpCode.NRefactory.CSharp.SyntaxTree tree = cpaser.Parse(classStr);
        AstNode classNode = tree.Children.First();
        return classNode;
    }
    private void PrintAstNode(AstNode item)
    {
        Debug.Log("[" + item.GetText() + "]" + " NodeType:" + item.NodeType + " Role:" + item.Role + " Type:" + item.GetType());
    }

    /// <summary>
    /// 测试查找调用函数
    /// </summary>
    [Test]
    public void TryFindAllInvocation()
    {
        var classNode = GetClassNode();
        var cons = classNode.Descendants.OfType<InvocationExpression>();
        foreach (var item in cons)
        {
            PrintAstNode(item);
        }
    }
    class FindInvocationsVisitor : DepthFirstAstVisitor
    {
        public override void VisitInvocationExpression(InvocationExpression invocationExpression)
        {
            Debug.Log(invocationExpression.Target);
            // Call the base method to traverse into nested invocations
            base.VisitInvocationExpression(invocationExpression);
        }
    }
    /// <summary>
    /// 测试使用Visitor进行查找
    /// </summary>
    [Test]
    public void VisitorTest()
    {
        ICSharpCode.NRefactory.CSharp.SyntaxTree syntaxTree = new CSharpParser().Parse(classStr);
        syntaxTree.AcceptVisitor(new FindInvocationsVisitor());
    }
    /// <summary>
    /// 测试修改一个方法的名称
    /// </summary>
    /// <param name="classNode"></param>
    [Test]
    public void TryChangeMethodName()
    {
        var classNode = GetClassNode();
        Debug.Log("Before:" + classNode.GetText());

        foreach (var item in classNode.Children)
        {
            PrintAstNode(item);
        }

        AstNodeCollection<EntityDeclaration> methods = classNode.GetChildrenByRole(Roles.TypeMemberRole);
        foreach (var method in methods)
        {
            if (!method.IsFrozen && method.Name == "DemoFunction")
            {
                Identifier identifier = method.NameToken;
                identifier.Name = "New_DemoFunction";
                Debug.Log("After:" + classNode.GetText());
            }
        }

    }

    /// <summary>
    /// 从无到有创建一个代码
    /// </summary>
    [Test]
    public void TryGenerateCode()
    {
        var document = new ICSharpCode.NRefactory.Editor.StringBuilderDocument("");
        var formattingOptions = FormattingOptionsFactory.CreateAllman();
        var options = new TextEditorOptions();
        using (var script = new DocumentScript(document, formattingOptions, options))
        {
            AstNode root = new ICSharpCode.NRefactory.CSharp.SyntaxTree();
            var usingSystem = new UsingDeclaration("System");
            root.AddChild<AstNode>(usingSystem, Roles.Root);
            var classNode = new TypeDeclaration();
            classNode.Name = "DemoClass";
            classNode.Modifiers = Modifiers.Public;
            root.AddChild(classNode, Roles.TypeMemberRole);

            var field = new FieldDeclaration();
            field.Modifiers = Modifiers.Public;
            field.Name = "int";// = Identifier.Create("int");
            //var member = new MemberType();
            //member.IsDoubleColon = false;
            //member.MemberName = "int";
            //field.ReturnType = new type("int");
            field.Variables.Add(new VariableInitializer("DemoField",new IdentifierExpression("0")));
            classNode.AddChild(field, Roles.TypeMemberRole);

            var constractNode = new MethodDeclaration();
            constractNode.Modifiers = Modifiers.Public;
            constractNode.Name = "DemoClass";
            constractNode.Body = new BlockStatement();
            classNode.AddChild(constractNode, Roles.TypeMemberRole);

            //classNode.Attributes.Add(new AttributeSection(new Attribute() { Arguments = { new IdentifierExpression("System.Serilizable") } }));
            //classNode.TypeParameters.Add(new TypeParameterDeclaration("T"));
            Debug.Log(usingSystem.GetText());
            var blockState = new BlockStatement();
            //blockState.AddChild<AstType>(usingSystem, Roles.em);
            script.InsertText(0, root.GetText());
        }
        Debug.Log(document.Text);
    }


}
//Roles

//Role

//Role<T>

//UsingDeclaration:AstNode

//MemberType: AstType : AstNode

//AstType : AstNode

//C# Syntax Tree 	Unresolved Type System	Resolved Type System
//AstType ITypeReference IType
//TypeDeclaration IUnresolvedTypeDefinition   ITypeDefinition
//EntityDeclaration   IUnresolvedEntity IEntity
//FieldDeclaration IUnresolvedField    IField
//PropertyDeclaration / IndexerDeclaration IUnresolvedProperty IProperty
//MethodDeclaration / ConstructorDeclaration / OperatorDeclaration /
//Accessor IUnresolvedMethod   IMethod
//EventDeclaration    IUnresolvedEvent IEvent
//Attribute IUnresolvedAttribute    IAttribute
//Expression  IConstantValue ResolveResult
//PrivateImplementationType IMemberReference    IMember
//ParameterDeclaration    IUnresolvedParameter IParameter
//Accessor IUnresolvedMethod   IMethod
//NamespaceDeclaration    UsingScope ResolvedUsingScope
//-	-	INamespace
//SyntaxTree  IUnresolvedFile 	-

//- 	IUnresolvedAssembly IAssembly
//- 	IProjectContent ICompilation