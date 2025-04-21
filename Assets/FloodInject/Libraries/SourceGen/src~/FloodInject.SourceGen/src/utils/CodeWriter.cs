using System;
using System.CodeDom.Compiler;
using System.IO;

public sealed class CodeWriter : IndentedTextWriter
{
    public class Scope : IDisposable
    {
        private CodeWriter _writer;
        private readonly string _prefix;
        private readonly string _inlinePrefix;
        private readonly string _postfix;
        private readonly string _inlinePostfix;
    
        public Scope(CodeWriter writer, string prefix = null, string inlinePrefix = null, string inlinePostfix = null, string postfix = null)
        {
            _writer = writer;
            _prefix = prefix;
            _inlinePostfix = inlinePostfix;
            _inlinePrefix = inlinePrefix;
            _postfix = postfix;
            Start();
        }

        ~Scope() => Dispose();
        
        public void Dispose()
        {
            End();
            _writer = null;
            System.GC.SuppressFinalize(this);
        }
        
        private void Start()
        {
            if (_writer != null)
            {
                if (_prefix != null)
                {
                    _writer.WriteLine(_prefix);
                }
                _writer.WriteLine(_inlinePrefix == null ? "{" : _inlinePrefix + "{");
                _writer.Indent++;
            }
        }

        private void End()
        {
            if (_writer != null)
            {
                _writer.Indent--;
                _writer.WriteLine(_inlinePostfix == null ? "}" : "}" + _inlinePostfix);
                if (_postfix != null)
                {
                    _writer.WriteLine(_postfix);
                }
            }
        }
    }
    
    public CodeWriter(TextWriter writer) : base(writer) { }

    public Scope CreateScope(
        string prefix = null, 
        string inlinePrefix = null, 
        string inlinePostfix = null,
        string postfix = null)
    {
        return new Scope(this, prefix, inlinePrefix, inlinePostfix, postfix);
    }

    public void StartNamespace(string @namespace)
    {
        if (string.IsNullOrEmpty(@namespace))
        {
            return;
        }
        WriteLine("namespace " + @namespace);
        WriteLine('{');
        Indent++;
    }
    
    public void EndNamespace(string @namespace)
    {
        if (string.IsNullOrEmpty(@namespace))
        {
            return;
        }
        Indent--;
        WriteLine('}');
    }
}