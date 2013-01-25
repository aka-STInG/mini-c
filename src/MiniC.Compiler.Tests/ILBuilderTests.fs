﻿module MiniC.Compiler.Tests.ILBuilderTests

open NUnit.Framework
open MiniC.Compiler
open MiniC.Compiler.IL

[<Test>]
let ``can build int return value``() =
    let program =
        [
            Ast.FunctionDeclaration(
                Ast.Void, "main", [],
                ([], [ Ast.ReturnStatement(
                        Some (Ast.LiteralExpression (Ast.IntLiteral 123))
                    )
                ])
            )
        ]
    let symbolEnvironment = SymbolEnvironment.create program
    let ilBuilder = new ILBuilder(symbolEnvironment)
    let result = ilBuilder.BuildClass program
    let expected =
        {
            Fields = [];
            Methods =
                [
                    {
                        Name       = "main";
                        ReturnType = typeof<System.Void>;
                        Parameters = [];
                        Locals     = [];
                        Body       = [ IL.Ldc_I4(123); IL.Ret ];
                    }
                ];
        }
    Assert.That(result, Is.EqualTo(expected))

[<Test>]
let ``can build binary expression``() =
    let program =
        [
            Ast.FunctionDeclaration(
                Ast.Void, "main", [],
                (
                    [],
                    [
                        Ast.ReturnStatement(
                            Some (
                                Ast.BinaryExpression(
                                    Ast.LiteralExpression (Ast.IntLiteral 123),
                                    Ast.Add,
                                    Ast.LiteralExpression (Ast.IntLiteral 456)
                                )
                            )
                        )
                    ]
                )
            )
        ]
    let symbolEnvironment = SymbolEnvironment.create program
    let ilBuilder = new ILBuilder(symbolEnvironment)
    let result = ilBuilder.BuildClass program
    let expected =
        {
            Fields = [];
            Methods =
                [
                    {
                        Name       = "main";
                        ReturnType = typeof<System.Void>;
                        Parameters = [];
                        Locals     = [];
                        Body       = [ IL.Ldc_I4(123); IL.Ldc_I4(456); IL.Add; IL.Ret ];
                    }
                ];
        }
    Assert.That(result, Is.EqualTo(expected))