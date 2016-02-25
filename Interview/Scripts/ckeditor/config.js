/**
 * @license Copyright (c) 2003-2016, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here.
	// For complete reference see:
	// http://docs.ckeditor.com/#!/api/CKEDITOR.config

	// The toolbar groups arrangement, optimized for two toolbar rows.
    config.toolbarGroups = [
        { name: 'pbckcode' },
		{ name: 'clipboard',   groups: [ 'clipboard', 'undo' ] },
		{ name: 'editing',     groups: [ 'find', 'selection', 'spellchecker' ] },
		{ name: 'links' },
		{ name: 'insert' },
		{ name: 'forms' },
		{ name: 'tools' },
		{ name: 'document',	   groups: [ 'mode', 'document', 'doctools' ] },
		{ name: 'others' },
		'/',
		{ name: 'basicstyles', groups: [ 'basicstyles', 'cleanup' ] },
		{ name: 'paragraph',   groups: [ 'list', 'indent', 'blocks', 'align', 'bidi' ] },
		{ name: 'styles' },
		{ name: 'colors' },
		{ name: 'about' }
	];

	// Remove some buttons provided by the standard plugins, which are
	// not needed in the Standard(s) toolbar.
	config.removeButtons = 'Underline,Subscript,Superscript';

	// Set the most common block elements.
	config.format_tags = 'p;h1;h2;h3;pre';

	// Simplify the dialog windows.
	config.removeDialogTabs = 'image:advanced;link:advanced';

	config.extraPlugins = 'pbckcode';

	config.pbckcode = {
	    // An optional class to your pre tag.
	    cls: '',

	    // The syntax highlighter you will use in the output view
	    highlighter: 'HIGHLIGHT',

	    // An array of the available modes for you plugin.
	    // The key corresponds to the string shown in the select tag.
	    // The value correspond to the loaded file for ACE Editor.
	    modes: [
            ['C/C++', 'c_pp'],
            ['C9Search', 'c9search'],
            ['Clojure', 'clojure'],
            ['CoffeeScript', 'coffee'],
            ['ColdFusion', 'coldfusion'],
            ['C#', 'csharp'],
            ['CSS', 'css'],
            ['Diff', 'diff'],
            ['Glsl', 'glsl'],
            ['Go', 'golang'],
            ['Groovy', 'groovy'],
            ['haXe', 'haxe'],
            ['HTML', 'html'],
            ['Jade', 'jade'],
            ['Java', 'java'],
            ['JavaScript', 'javascript'],
            ['JSON', 'json'],
            ['JSP', 'jsp'],
            ['JSX', 'jsx']
            ['LaTeX', 'latex'],
            ['LESS', 'less'],
            ['Liquid', 'liquid'],
            ['Lua', 'lua'],
            ['LuaPage', 'luapage'],
            ['Markdown', 'markdown'],
            ['OCaml', 'ocaml'],
            ['Perl', 'perl'],
            ['pgSQL', 'pgsql'],
            ['PHP', 'php'],
            ['Powershell', 'powershel1'],
            ['Python', 'python'],
            ['R', 'ruby'],
            ['OpenSCAD', 'scad'],
            ['Scala', 'scala'],
            ['SCSS/Sass', 'scss'],
            ['SH', 'sh'],
            ['SQL', 'sql'],
            ['SVG', 'svg'],
            ['Tcl', 'tcl'],
            ['Text', 'text'],
            ['Textile', 'textile'],
            ['XML', 'xml'],
            ['XQuery', 'xq'],
            ['YAML', 'yaml']

	    ],

	    // The theme of the ACE Editor of the plugin.
	    theme: 'textmate',

	    // Tab indentation (in spaces)
	    tab_size: '4'
	};


};
