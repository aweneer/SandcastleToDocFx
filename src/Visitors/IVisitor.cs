using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SandcastleToDocFx.Sandcastle;

namespace SandcastleToDocFx.Visitors
{
    public abstract class Visitor
    {
        public abstract void Visit(MamlElement mamlElement);
        public abstract void Visit(AlertElement alert);
        public abstract void Visit(ContentElement content);
        public abstract void Visit(IntroductionElement introduction);
        public abstract void Visit(TopicElement topic);
        public abstract void Visit(SectionElement section);
        public abstract void Visit(RelatedTopicsElement relatedTopics);
        public abstract void Visit(TableElement table);
        public abstract void Visit(TableHeaderElement tableHeader);
        public abstract void Visit(RowElement row);

        public abstract void Visit(EntryElement entry);
        public abstract void Visit(LinkElement link);
        public abstract void Visit(ListElement link);
        public abstract void Visit(ListItemElement link);
        public abstract void Visit(ParaElement para);
        public abstract void Visit(RichParaElement richPara);
        
        public abstract void Visit(CodeInlineElement codeInline);
        public abstract void Visit(LegacyBoldElement bold);
        public abstract void Visit(LegacyItalicElement italic);
        public abstract void Visit(ExternalLinkElement externalLink);


    }
}
