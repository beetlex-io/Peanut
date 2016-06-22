using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using System.CodeDom;
using System.CodeDom.Compiler;
using DDW;
using DDW.Collections;
using System.IO;
namespace Peanut.Generate
{
    public class GenerateCode
    {

        //CodeCompileUnit mEntityUnit = new CodeCompileUnit();
        CompilationUnitNode mEntityUnit = new CompilationUnitNode();
        string mTableName;
        string mClassName;
        public GenerateCode()
        {



        }
        public System.IO.Stream Builder(System.IO.Stream stream)
        {
           
            CodeDomProvider proider = new Microsoft.CSharp.CSharpCodeProvider();
            System.IO.StringWriter sw = new System.IO.StringWriter();
            Console.SetOut(sw);
            Console.SetError(sw);
            // CSharpParser p = new CSharpParser(null, stream, null);
            System.IO.Stream result = new System.IO.MemoryStream();
            System.IO.StreamWriter writer = new System.IO.StreamWriter(result, Encoding.UTF8);
            CompilationUnitNode unit = null;
            using (System.IO.StreamReader sr = new StreamReader(stream, Encoding.UTF8))
            {
                Lexer lexer = new Lexer(sr);
                TokenCollection tc = lexer.Lex();
                Parser parser = new Parser();
                unit = parser.Parse(tc, lexer.StringLiterals);

            }

            if (unit != null)
            {
                try
                {
                   
                    Microsoft.CSharp.CSharpCodeProvider csp = new Microsoft.CSharp.CSharpCodeProvider();
                    foreach (UsingDirectiveNode item in unit.DefaultNamespace.UsingDirectives)
                    {
                        mEntityUnit.DefaultNamespace.UsingDirectives.Add(item);
                    }
                    foreach (NamespaceNode cn in unit.Namespaces)
                    {
                        BuilderNameSpace(cn, unit);
                    }   
                    StringBuilder sb = new StringBuilder();
                    mEntityUnit.ToSource(sb);
                    writer.Write(sb.ToString());

                }
                catch (Exception e_)
                {
                    writer.WriteLine(e_.Message + e_.StackTrace);
                }
                writer.Flush();
            }
            return result;

        }
        private void BuilderNameSpace(NamespaceNode cn, CompilationUnitNode unit)
        {


            NamespaceNode nspace = new NamespaceNode(new Token(TokenID.Namespace));
            nspace.Name = cn.Name;
            mEntityUnit.Namespaces.Add(nspace);
            foreach (InterfaceNode item in cn.Interfaces)
            {
                BuilderType(item,nspace);
            }

            


        }

        private string GetTableName(InterfaceNode type)
        {
            string mClassName = type.Name.Identifier.Substring(1, type.Name.Identifier.Length - 1);
            foreach (AttributeNode attr in type.Attributes)
            {
               
                if (attr.Name.GenericIdentifier.ToLower() == "table")
                {
                    if (attr.Arguments.Count > 0)
                    {

                        DDW.StringPrimitive pe = (DDW.StringPrimitive)attr.Arguments[0].Expression;
                        if (pe != null)
                        {
                           return pe.Value.ToString();
                        }
                        else
                        {
                            return mClassName;

                        }

                    }
                    else
                    {
                        return  mClassName;
                    }
                }
            }
             return  mClassName;
        }

        private string GetFieldName(InterfacePropertyNode property)
        {
            string fieldname=null;
            foreach (AttributeNode attr in property.Attributes)
            {
                if (attr.Name.GenericIdentifier.ToLower() == "id" || attr.Name.GenericIdentifier.ToLower() == "column")
                {
                    if (attr.Arguments.Count > 0)
                    {

                        DDW.StringPrimitive pe = (DDW.StringPrimitive)attr.Arguments[0].Expression;
                        if (pe != null)
                        {
                            fieldname = pe.Value.ToString();
                        }
                        else
                        {
                            fieldname = property.Names[0].GenericIdentifier;

                        }

                    }
                    else
                    {
                        fieldname = property.Names[0].GenericIdentifier;
                    }
                }
            }
            return fieldname;
        }

        private void BuilderType(InterfaceNode type,NamespaceNode nspace)
        {
            DDW.ClassNode cls = new ClassNode(new Token(TokenID.Public));
            cls.Modifiers = Modifier.Public;
            cls.BaseClasses.Add(new TypeNode(new IdentifierExpression("Peanut.Mappings.DataObject", new Token(TokenID.Typeof))));
            foreach (AttributeNode attr in type.Attributes)
            {
                cls.Attributes.Add(attr);
            }
            mTableName = GetTableName(type);
            mClassName = type.Name.Identifier.Substring(1, type.Name.Identifier.Length - 1);
            cls.Name = new IdentifierExpression(mClassName, new Token(TokenID.String|TokenID.Public));
            cls.IsPartial = true;
            nspace.Classes.Add(cls);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("///<summary>");
            sb.AppendLine("///Peanut Generator Copyright @ FanJianHan 2010-2013");
            sb.AppendLine("///website:http://www.ikende.com");
            if (!string.IsNullOrEmpty(type.Comment))
            {
                sb.AppendLine(type.Comment);
            }
            StringReader sr = new StringReader(type.DocComment);
            string value = sr.ReadLine();
            while (value != null)
            {
                if (value.IndexOf("summary>") == -1)
                {
                    sb.AppendLine(value);
                }
                value = sr.ReadLine();
            }
            sb.AppendLine("///</summary>");
            cls.DocComment = sb.ToString();

            foreach (InterfacePropertyNode property in type.Properties)
            {
                string propertyname = property.Names[0].GenericIdentifier;
                string fieldname= GetFieldName(property);
                FieldNode field = new FieldNode(new Token(TokenID.False));
                field.Modifiers = Modifier.Private;
                QualifiedIdentifierExpression name = new QualifiedIdentifierExpression(new Token(TokenID.String));
                name.Expressions.Add(new IdentifierExpression("m" + property.Names[0].GenericIdentifier, new Token(TokenID.String)));
                field.Names.Add(name);
                field.Type = property.Type;
                cls.Fields.Add(field);

                IType fieldtype=new TypeNode(new IdentifierExpression("Peanut.FieldInfo<"+((TypeNode)property.Type).GenericIdentifier+">", new Token(TokenID.Typeof)));
                field = new FieldNode(new Token(TokenID.False));
                field.Modifiers = Modifier.Public| Modifier.Static;
               
                NodeCollection<ArgumentNode> args = new NodeCollection<ArgumentNode>();
                args.Add(new ArgumentNode(new Token(TokenID.String)));
                args.Add(new ArgumentNode(new Token(TokenID.String)));
                args[0].Expression = new StringPrimitive(mTableName, new Token(TokenID.String));
                args[1].Expression = new StringPrimitive(fieldname, new Token(TokenID.String));

                name = new QualifiedIdentifierExpression(new Token(TokenID.String));
                name.Expressions.Add(new AssignmentExpression(TokenID.Equal,
                    new IdentifierExpression(propertyname.Substring(0, 1).ToLower() + propertyname.Substring(1, propertyname.Length - 1)
                    , new Token(TokenID.String)), new ObjectCreationExpression(fieldtype, args, new Token(TokenID.New))));
                field.Names.Add(name);
                field.Type = fieldtype;
               
                cls.Fields.Add(field);


                PropertyNode pn = new PropertyNode(new Token(TokenID.Newline));
                foreach (AttributeNode attr in property.Attributes)
                {
                    pn.Attributes.Add(attr);
                }
                pn.Names = property.Names;
                pn.Modifiers = Modifier.Public | Modifier.Virtual;
                pn.Type = property.Type;
                pn.Setter = new AccessorNode(true, new Token(TokenID.Newline));
                pn.Setter.Kind = "set";
                ExpressionStatement setvalue = new ExpressionStatement(
                    new AssignmentExpression(TokenID.Equal,
                    new IdentifierExpression("m" + property.Names[0].GenericIdentifier, new Token(TokenID.String))
                    , new IdentifierExpression("value", new Token(TokenID.String)))
                    );
                pn.Setter.StatementBlock.Statements.Add(setvalue);

                args = new NodeCollection<ArgumentNode>();
                args.Add(new ArgumentNode(new Token(TokenID.String)));
                args[0].Expression = new StringPrimitive(propertyname, new Token(TokenID.String));
                QualifiedIdentifierExpression invoke = new QualifiedIdentifierExpression(new Token(TokenID.String));
                invoke.Expressions.Add(new IdentifierExpression("EntityState", new Token(TokenID.String)));
                invoke.Expressions.Add(new InvocationExpression(new IdentifierExpression("FieldChange", new Token(TokenID.Default)), args));
                setvalue = new ExpressionStatement(invoke);
                pn.Setter.StatementBlock.Statements.Add(setvalue);

                pn.Getter = new AccessorNode(true, new Token(TokenID.Newline));
                pn.Getter.Kind = "get";
                ReturnStatement rs = new ReturnStatement(new Token(TokenID.Return));
                rs.ReturnValue = new IdentifierExpression("m" + property.Names[0].GenericIdentifier, new Token(TokenID.String));
                pn.Getter.StatementBlock.Statements.Add(rs);

                sb = new StringBuilder();
                sb.AppendLine("///<summary>");
                sb.AppendLine("///Type:" + ((TypeNode)property.Type).GenericIdentifier);
                if (!string.IsNullOrEmpty(property.Comment))
                {
                    sb.AppendLine(type.Comment);
                }
                sr = new StringReader(property.DocComment);
                value = sr.ReadLine();
                while (value != null)
                {
                    if (value.IndexOf("summary>") == -1)
                    {
                        sb.AppendLine(value);
                    }
                    value = sr.ReadLine();
                }
                sb.AppendLine("///</summary>");
                pn.DocComment = sb.ToString();
                

                cls.Properties.Add(pn);
                
            }
            //CodeTypeDeclaration entity = new CodeTypeDeclaration(mClassName);
            //CodeCommentStatement comm;
            //cns.Types.Add(entity);

           
            //comm = new CodeCommentStatement("<summary>");
            //comm.Comment.DocComment = true;
            //entity.Comments.Add(comm);
            //comm = new CodeCommentStatement("Peanut Generator Copyright © FanJianHan 2010-2013");
            //comm.Comment.DocComment = true;
            //entity.Comments.Add(comm);
            //comm = new CodeCommentStatement("website:http://www.ikende.com");
            //comm.Comment.DocComment = true;
            //entity.Comments.Add(comm);
            //if (!string.IsNullOrEmpty(type.Comment))
            //{
            //    comm = new CodeCommentStatement(type.Comment);
            //    comm.Comment.DocComment = true;
            //    entity.Comments.Add(comm);
            //}

            //StringReader sr = new StringReader(type.DocComment);
            //string value = sr.ReadLine();
            //while (value != null)
            //{
            //    if (value.IndexOf("summary>") == -1)
            //    {
            //        comm = new CodeCommentStatement(value.Replace("///", ""));
            //        comm.Comment.DocComment = true;
            //        entity.Comments.Add(comm);
            //    }
            //    value = sr.ReadLine();
            //}
            //comm = new CodeCommentStatement("</summary>");
            //comm.Comment.DocComment = true;
            //entity.Comments.Add(comm);
            //// }
            //entity.BaseTypes.Add(new CodeTypeReference("Peanut.Mappings.DataObject"));
            //entity.BaseTypes.Add(new CodeTypeReference(type.Name.Identifier));
            //entity.Attributes = MemberAttributes.Public;
            //entity.IsPartial = true;
            //entity.CustomAttributes.Add(new CodeAttributeDeclaration("Serializable"));
            //entity.IsClass = true;
            //foreach (AttributeNode aitem in type.Attributes)
            //{
            //    CodeAttributeDeclaration attribute = new CodeAttributeDeclaration(aitem.Name.GenericIdentifier);
            //    entity.CustomAttributes.Add(attribute);
            //    if (attribute.Name.ToLower() == "table")
            //    {
            //        if (aitem.Arguments.Count > 0)
            //        {

            //            DDW.StringPrimitive pe = (DDW.StringPrimitive)aitem.Arguments[0].Expression;
            //            if (pe != null)
            //            {
            //                mTableName = pe.Value.ToString();
            //            }
            //            else
            //            {
            //                mTableName = mClassName;

            //            }
            //            attribute.Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(mTableName)));
            //        }
            //        else
            //        {
            //            mTableName = mClassName;
            //        }
            //    }

            //}
            //foreach (InterfacePropertyNode mitem in type.Properties)
            //{


            //    BuilderProperty(entity, mitem);

            //}



        }

        private CodeTypeReference GetTypeRef(string name)
        {
            switch (name.ToLower())
            {
                case "string":
                    return new CodeTypeReference(typeof(string));
                case "int":
                    return new CodeTypeReference(typeof(int));
                case "short":
                    return new CodeTypeReference(typeof(short));
                case "float":
                    return new CodeTypeReference(typeof(float));
                case "double":
                    return new CodeTypeReference(typeof(double));
                case "bool":
                    return new CodeTypeReference(typeof(bool));
                case "long":
                    return new CodeTypeReference(typeof(long));
                default:
                    return new CodeTypeReference(name);
            }
        }

        private void BuilderProperty(CodeTypeDeclaration entity, InterfacePropertyNode property)
        {
            TypeNode type = (TypeNode)property.Type;
            string propertyname = property.Names[0].GenericIdentifier;

            CodeMemberField field = new CodeMemberField(GetTypeRef(type.GenericIdentifier), "m" +propertyname );
            CodeMemberField fieldinfo = new CodeMemberField();
            field.Attributes = MemberAttributes.Private;
            entity.Members.Add(field);
            CodeCommentStatement comm;
            CodeMemberProperty mp = new CodeMemberProperty();
           
                comm = new CodeCommentStatement("<summary>");
                comm.Comment.DocComment = true;
                fieldinfo.Comments.Add(comm);
                comm = new CodeCommentStatement(type.GenericIdentifier);
                comm.Comment.DocComment = true;
                fieldinfo.Comments.Add(comm);

             if (!string.IsNullOrEmpty(property.Comment))
                    {
                        comm = new CodeCommentStatement(property.Comment);
                        comm.Comment.DocComment = true;
                        fieldinfo.Comments.Add(comm);
                    }

                    StringReader sr = new StringReader(property.DocComment);
                    string value= sr.ReadLine();
                    while (value != null)
                    {
                        if (value.IndexOf("summary>") == -1)
                        {
                            comm = new CodeCommentStatement(value.Replace("///",""));
                            comm.Comment.DocComment = true;
                            fieldinfo.Comments.Add(comm);
                        }
                        value = sr.ReadLine();
                    }

                comm = new CodeCommentStatement("</summary>");
                comm.Comment.DocComment = true;
                fieldinfo.Comments.Add(comm);
            
            mp.Name = propertyname.Substring(0, 1).ToUpper() + propertyname.Substring(1, propertyname.Length - 1);
            string columnName = propertyname;
            foreach (AttributeNode attribute in property.Attributes)
            {
                CodeAttributeDeclaration dad = new CodeAttributeDeclaration(attribute.Name.GenericIdentifier);
                if (attribute.Arguments.Count > 0)
                {
                    foreach (AttributeArgumentNode aan in attribute.Arguments)
                    {
                        ExpressionNode pe = aan.Expression;
                        System.Reflection.PropertyInfo pi = pe.GetType().GetProperty("Value");
                        object data = pi.GetValue(pe, null);
                        dad.Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(data)));
                    }
                }
                mp.CustomAttributes.Add(dad);
                
            }
            //foreach (CodeAttributeDeclaration aitem in property.CustomAttributes)
            //{
            //    mp.CustomAttributes.Add(aitem);
            //    if (aitem.Name.ToLower() == "id" || aitem.Name.ToLower() == "column")
            //    {
            //        if (aitem.Arguments.Count > 0)
            //        {
            //            CodePrimitiveExpression pe = aitem.Arguments[0].Value as CodePrimitiveExpression;
            //            if (pe != null)
            //            {
            //                columnName = pe.Value.ToString();
            //            }
            //            else
            //            {
            //                columnName = property.Name;

            //            }
            //        }
            //        else
            //        {
            //            columnName = property.Name;
            //        }
            //    }
            //}
            mp.Attributes = MemberAttributes.Public;
            mp.Type = GetTypeRef(type.GenericIdentifier);
            entity.Members.Add(mp);
            mp.GetStatements.Add(
                new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name)));

            mp.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name),
                                    new CodePropertySetValueReferenceExpression()));
            mp.SetStatements.Add(
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "EntityState"), "FieldChange"), new CodePrimitiveExpression(propertyname)));


            fieldinfo.Type = new CodeTypeReference("Peanut.FieldInfo");
            fieldinfo.Name = propertyname.Substring(0, 1).ToLower() + propertyname.Substring(1, propertyname.Length - 1);
            fieldinfo.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            entity.Members.Add(fieldinfo);
            fieldinfo.InitExpression = new CodeObjectCreateExpression(
                new CodeTypeReference("Peanut.FieldInfo"), new CodePrimitiveExpression(mTableName), new CodePrimitiveExpression(columnName));


        }


    }
}
