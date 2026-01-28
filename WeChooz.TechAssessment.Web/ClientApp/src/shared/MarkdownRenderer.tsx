import { Box } from "@mantine/core";

export function MarkdownRenderer({ html }: { html: string }) {
    return (
        <Box className="markdown-content">
            <style>
                {`
                .markdown-content { line-height: 1.6; }
                .markdown-content h1 { font-size: 1.75rem; margin: 1.25rem 0 0.75rem; }
                .markdown-content h2 { font-size: 1.35rem; margin: 1.1rem 0 0.6rem; }
                .markdown-content h3 { font-size: 1.15rem; margin: 0.9rem 0 0.5rem; }
                .markdown-content p { margin: 0.6rem 0; }
                .markdown-content ul { padding-left: 1.25rem; margin: 0.6rem 0; }
                .markdown-content ol { padding-left: 1.25rem; margin: 0.6rem 0; }
                .markdown-content li { margin: 0.35rem 0; }
                .markdown-content code { background: #f3f4f6; padding: 0 0.25rem; border-radius: 4px; }
                .markdown-content pre { background: #f3f4f6; padding: 0.75rem; border-radius: 6px; overflow-x: auto; }
                .markdown-content blockquote { border-left: 3px solid #e5e7eb; padding-left: 0.75rem; color: #6b7280; }
                `}
            </style>
            <div dangerouslySetInnerHTML={{ __html: html }} />
        </Box>
    );
}
