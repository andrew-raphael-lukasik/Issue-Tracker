using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.IO;


//TODO:
//
// - czy zapisywanie plikównie da się zrobić jakoś przez unity? np. AssetDatabase.SaveAssets() itp
// - lepsza obsługa zapisu (w niektórych sytuacja wyrzuca errory)
// - parent/hierarchical/sub tasks
// - uporządkować cały kod
// - przenieść do DLL?
//

namespace IssueTrackerProgram
{

    class IssueTracker : EditorWindow
    {
        #region FIELDS

        public static IssueTracker instance;

        public const string EDITOR_PREF_USER_KEY = "issue tracker user name (#gq438gqh8gh93g9HeWj00w30k3)";
        public const string STR_no_one = "no one";
        public const string STR_Guest = "Guest";
        public const string STR_user_name = "user name:";
        public const string STR_Create_New_Entry = "Create New Entry";
        public const string STR_show_closed = "show closed";
        public const string STR_assigned_to = "assigned to: ";
        public const string STR_lock = "  lock";
        public const string STR_locked = "  locked";
        public const string STR_release = "  release";
        public const string STR_ToDo = "ToDo";
        public const string STR_dot_todo = ".todo";
        public const string STR_slash_ToDo_slash = "/ToDo/";
        public const string STR_Assets = "Assets";
        public const string STR_Assets_slash = "Assets/";
        public const string STR_Assets_slash_ToDo = "Assets/ToDo";
        public const string STR_Create = "Create";
        public const string STR_New_Issue = "New Issue";
        public const string STR_message = "messsage";
        public const string STR_change_user_name = "Change User Name";
        public const string STR_apply = "Apply";
        public const string STR_entry_details = "Entry Details";
        public const string STR_created_by = "created by: ";
        public const string STR_delete_entry = "delete entry";
        public const string STR_close = "close";
        public const string STR_select_entry_to_see_details = "select entry to see details";
        public const string STR_yes = "Yes";
        public const string STR_no = "No";
        public const string STR_ToDo_slash_Open = "ToDo/Open";

        public readonly static GUILayoutOption heightX2 = GUILayout.Height( EditorGUIUtility.singleLineHeight*2f );
        public readonly static GUILayoutOption heightX3 = GUILayout.Height( EditorGUIUtility.singleLineHeight*3f );
        public readonly static GUILayoutOption width60 = GUILayout.Width( 60f );
        public readonly static GUILayoutOption width80 = GUILayout.Width( 80f );
        public readonly static GUILayoutOption width160 = GUILayout.Width( 160f );

        public readonly static Color colorBg1 = new Color( 0.5f , 0.5f , 0.5f );
        public readonly static Color colorText1 = new Color( 0.23f , 1f , 1f );
        public readonly static Color colorText2 = new Color( 1f , 0.75f , 0.1f );
        public readonly static Color colorGreyDark = new Color( 0.3f , 0.3f , 0.3f );

        public readonly static float singleLineHeight = EditorGUIUtility.singleLineHeight;

        public Texture2D icon_reload;
        public Texture2D icon_locked;
        public Texture2D icon_unlocked;
        public Texture2D icon_search;
        public Texture2D icon_window;

        /// <summary> keys is GUID of .todo file and values is a Entry object </summary>
        public static List<EntryAndFileGUID> _entries = new List<EntryAndFileGUID>();

        public Vector2 _scrollViewPosition;
        bool _isWindowWide;
        [SerializeField] bool _showClosed;
        [SerializeField] string _filterByText = "";

        #endregion
        #region EDITOR_WINDOW_METHODS

        void OnEnable ()
        {
			
            //
            instance = this;
			
            //make sure ToDo directory exists
            if( AssetDatabase.IsValidFolder( STR_Assets_slash_ToDo ) == false )
            {
                AssetDatabase.CreateFolder( STR_Assets , STR_ToDo );
            }

            {
                //define color palette:
                Color _ = Color.clear;
                Color o = colorText1*0.8f;
                Color[] PXL_icon = new Color[]
                {
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    _,
                    o,
                    o,
                    o,
                    _,
                    _,
                    _,
                    _,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    _,
                    _,
                    _,
                    o,
                    o,
                    o,
                    o,
                    _,
                    o,
                    o,
                    o,
                    _,
                    _,
                    o,
                    o,
                    o,
                    _,
                    _,
                    _,
                    o,
                    o,
                    o,
                    _,
                    o,
                    o,
                    _,
                    _,
                    _,
                    _,
                    _,
                    o,
                    o,
                    _,
                    o,
                    o,
                    _,
                    _,
                    _,
                    _,
                    _,
                    o,
                    o,
                    o,
                    o,
                    o,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    o,
                    o,
                    o,
                    o,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                };
                Color[] PXL_reload = new Color[]
                {
                    _,
                    _,
                    o,
                    o,
                    o,
                    o,
                    o,
                    _,
                    _,
                    _,
                    _,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    _,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    _,
                    _,
                    _,
                    _,
                    o,
                    o,
                    o,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    _,
                    _,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    _,
                    _,
                    _,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    _,
                    o,
                    _,
                    _,
                    o,
                    o,
                    o,
                    o,
                    o,
                    _,
                    _,
                };
                Color[] PXL_locked = new Color[]
                {
                    _,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    _,
                    _,
                    _,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    _,
                    _,
                    _,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    _,
                    _,
                    _,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    o,
                    _,
                    _,
                    _,
                    _,
                    _,
                    o,
                    _,
                    _,
                    _,
                    o,
                    _,
                    _,
                    _,
                    _,
                    _,
                    o,
                    _,
                    _,
                    _,
                    o,
                    _,
                    _,
                    _,
                    _,
                    _,
                    o,
                    _,
                    _,
                    _,
                    _,
                    o,
                    o,
                    o,
                    o,
                    o,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                };
                Color[] PXL_unlocked = new Color[]
                {
                    _,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    _,
                    _,
                    _,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    _,
                    _,
                    _,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    _,
                    _,
                    _,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    o,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    o,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    o,
                    _,
                    _,
                    _,
                    o,
                    _,
                    _,
                    _,
                    _,
                    _,
                    o,
                    _,
                    _,
                    _,
                    _,
                    o,
                    o,
                    o,
                    o,
                    o,
                    _,
                    _,
                    _,
                };
                Color[] PXL_search = new Color[]
                {
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    o,
                    o,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    o,
                    o,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    o,
                    o,
                    _,
                    _,
                    _,
                    _,
                    o,
                    o,
                    o,
                    o,
                    o,
                    o,
                    _,
                    _,
                    _,
                    o,
                    _,
                    _,
                    _,
                    _,
                    o,
                    _,
                    _,
                    _,
                    o,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    o,
                    _,
                    _,
                    o,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    o,
                    _,
                    _,
                    o,
                    _,
                    _,
                    _,
                    _,
                    _,
                    _,
                    o,
                    _,
                    _,
                    _,
                    o,
                    _,
                    _,
                    _,
                    _,
                    o,
                    _,
                    _,
                    _,
                    _,
                    _,
                    o,
                    o,
                    o,
                    o,
                    _,
                    _,
                    _,
                    _,
                };

                //
                if( icon_window == null )
                {
                    icon_window = new Texture2D( 10 , 10 , TextureFormat.RGBAHalf , true );
                    icon_window.SetPixels( PXL_icon );
                }

                //create refresh button's texture:
                if( icon_reload == null )
                {
                    icon_reload = new Texture2D( 10 , 10 , TextureFormat.RGBAHalf , true );
                    icon_reload.SetPixels( PXL_reload );
                }

                //create locked button's texture:
                if( icon_locked == null )
                {
                    icon_locked = new Texture2D( 10 , 10 , TextureFormat.RGBAHalf , true );
                    icon_locked.SetPixels( PXL_locked );
                }

                //create unlocked button's texture:
                if( icon_unlocked == null )
                {
                    icon_unlocked = new Texture2D( 10 , 10 , TextureFormat.RGBAHalf , true );
                    icon_unlocked.SetPixels( PXL_unlocked );
                }

                //search icon:
                if( icon_search == null )
                {
                    icon_search = new Texture2D( 10 , 10 , TextureFormat.RGBAHalf , true );
                    icon_search.SetPixels( PXL_search );
                }

            }

            //set title content:
            this.titleContent = new GUIContent( STR_ToDo , icon_window );

            //refresh window:
            EntriesReload();
            this.Repaint();

        }

        void OnGUI ()
        {

            try
            {

                //assess _isWindowWide:
                _isWindowWide = EditorGUIUtility.currentViewWidth > 350f;

                GUILayout.Space( 4f );

                //search box:
                GUILayout.BeginHorizontal();
                {

                    GUILayout.FlexibleSpace();

                    GUI.contentColor = colorText1;
                    GUILayout.Button( icon_search );
                    //GUILayout.Label( "search: " , width60 );
                    GUI.backgroundColor = _filterByText.Length == 0 ? Color.grey : colorText1*2f;
                    GUI.contentColor = colorText1;
                    _filterByText = EditorGUILayout.TextField( _filterByText , GUILayout.Width( EditorGUIUtility.currentViewWidth*0.4f ) );
                    GUI.contentColor = Color.white;
                    GUI.backgroundColor = Color.white;

                    GUILayout.FlexibleSpace();

                    //refresh button:
                    GUI.backgroundColor = Color.grey;
                    if( GUILayout.Button( icon_reload ) )
                    {
                        EntriesReload();
                    }
                    GUI.backgroundColor = Color.white;

                    GUILayout.Space( 12f );
                }
                GUILayout.EndHorizontal();

                GUILayout.Space( 2f );
                GuiDrawSeparator_A( colorGreyDark );

                //wczytaj wszystkie wpisy i je wyświetl:
                {
                    //EditorGUILayout.Space();
                    _scrollViewPosition = EditorGUILayout.BeginScrollView( _scrollViewPosition );

                    //draw separator line:
                    //GuiDrawSeparator_A( colorGreyDark );

                    foreach( var item in _entries )
                    {

                        //handle filter by text:
                        if( _filterByText.Length == 0 || FilterEntryByString( item.entry , _filterByText ) )
                        {

                            //handle showClosed:
                            if( _showClosed == true || item.entry.state != Entry.State.done && item.entry.state != Entry.State.canceled )
                            {

                                //
                                GuiDrawSeparator_B( Color.white );

                                //
                                GUI.backgroundColor = item.entry.CalculateColor();

                                GUILayout.BeginHorizontal( GUILayout.Width( 240f ) );
                                {

                                    //address _isWindowWide:
                                    if( _isWindowWide == false )
                                    {
                                        EditorGUILayout.BeginVertical();
                                    }

                                    //
                                    if( item.entry.state == Entry.State.done )
                                    {
                                        GUI.backgroundColor = new Color( GUI.backgroundColor.r , 1f , GUI.backgroundColor.b );
                                    }

                                    GUILayout.BeginHorizontal();
                                    {

                                        //debug list sorting score:
                                        //GUILayout.Label( item.entry.sortingScore.ToString() );

                                        //draw entry text description:
                                        if( GUILayout.Button( item.entry.title , GUILayout.Width( _isWindowWide ? EditorGUIUtility.currentViewWidth-192f-( singleLineHeight*2f ) : EditorGUIUtility.currentViewWidth-30f-( singleLineHeight*2f ) ) , heightX2 ) )
                                        {
                                            EntryDetailsWindow.CreateWindow( item );
                                        }

                                        //draw subject preview icon:
                                        {
                                            //lasy way to get last used Rect (draw button and use GUILayoutUtility):
                                            if( GUILayout.Button( GUIContent.none , GUILayout.Width( singleLineHeight*2f ) , heightX2 ) )
                                            {
                                                if( item.entry.subject != null )
                                                {
                                                    Selection.activeObject = AssetDatabase.LoadMainAssetAtPath( AssetDatabase.GetAssetPath( item.entry.subject ) ) as Object;
                                                }
                                            }
                                            Rect lastRect = GUILayoutUtility.GetLastRect();
                                            if( item.entry.subjectPreview != null )
                                            {
                                                EditorGUI.DrawTextureTransparent( lastRect , item.entry.subjectPreview );
                                            }
                                            //slightly colorize this preview icon:
                                            Color saveColor = GUI.backgroundColor;
                                            GUI.backgroundColor = new Color( GUI.backgroundColor.r , GUI.backgroundColor.g , GUI.backgroundColor.b , 0.4f );
                                            GUI.Button( lastRect , GUIContent.none );
                                            GUI.backgroundColor = saveColor;
                                        }

                                    }
                                    GUILayout.EndHorizontal();

                                    EditorGUI.BeginChangeCheck();

                                    EditorGUILayout.BeginVertical();
                                    {
                                        GUILayout.BeginHorizontal();
                                        {
                                            Color prevColor = GUI.contentColor;
                                            GUI.contentColor = colorText2;
                                            item.entry.type = (Entry.Type)EditorGUILayout.EnumPopup( (System.Enum)item.entry.type );
                                            item.entry.priority = (Entry.Priority)EditorGUILayout.EnumPopup( (System.Enum)item.entry.priority );
                                            item.entry.state = (Entry.State)EditorGUILayout.EnumPopup( (System.Enum)item.entry.state );
                                            GUI.contentColor = prevColor;
                                        }
                                        GUILayout.EndHorizontal();

                                        //
                                        GUI.contentColor = Color.grey;
                                        item.entry.subject = EditorGUILayout.ObjectField( item.entry.subject , typeof(Object) , false );
                                        if( EditorGUI.EndChangeCheck() )
                                        {
                                            if( item.entry.state == Entry.State.done || item.entry.state == Entry.State.canceled )
                                            {
                                                item.entry.assignedTo = STR_no_one;
                                            }
                                            item.entry.UpdatePrevievTexture();
                                            EntrySave( item );
                                        }
                                        GUI.contentColor = Color.white;

                                        //address _isWindowWide:
                                        if( _isWindowWide == false )
                                        {
                                            EditorGUILayout.EndVertical();  
                                        }

                                    }
                                    EditorGUILayout.EndVertical();


                                }
                                GUILayout.EndHorizontal();

                                //
                                GUILayout.BeginHorizontal();
                                {
                                    GUILayout.Label( STR_assigned_to+item.entry.assignedTo );

                                    GUILayout.FlexibleSpace();

                                    //lock/unlock buton:
                                    if( item.entry.assignedTo != UserNameLoad() )
                                    {
                                        if( item.entry.assignedTo != STR_no_one )
                                        {
                                            Color saveColor = GUI.backgroundColor;
                                            GUI.backgroundColor = Color.white;
                                            GUILayout.Button( new GUIContent( STR_locked , icon_locked ) , GUILayout.MaxWidth( 200f ) , GUILayout.ExpandWidth( true ) );
                                            GUI.backgroundColor = saveColor;
                                        } else
                                        {
                                            if( GUILayout.Button( new GUIContent( STR_lock , icon_locked ) , GUILayout.MaxWidth( 200f ) , GUILayout.ExpandWidth( true ) ) )
                                            {
                                                //
                                                item.entry.assignedTo = UserNameLoad();
                                                //
                                                EntrySave( item );
                                                SortEntries();
                                                return;
                                            }
                                        }
                                    } else
                                    {
                                        Color saveColor = GUI.backgroundColor;
                                        GUI.backgroundColor = colorText1;
                                        if( GUILayout.Button( new GUIContent( STR_release , icon_unlocked ) , GUILayout.MaxWidth( 200f ) , GUILayout.ExpandWidth( true ) ) )
                                        {
                                            //
                                            item.entry.assignedTo = STR_no_one;
                                            //
                                            EntrySave( item );
                                            SortEntries();
                                            return;
                                        }
                                        GUI.backgroundColor = saveColor;
                                    }

                                }
                                GUILayout.EndHorizontal();

                                //reset bg color:
                                GUI.backgroundColor = Color.white;

                                //draw separator line:
                                GuiDrawSeparator_A( Color.black );

                            }

                        }

                    }
                    EditorGUILayout.EndScrollView();
                }

                GUILayout.FlexibleSpace();
                GuiDrawSeparator_A( colorText1*3f ); // new Color( 1f , 1f , 1f , 0.3f ) );

                //nazwa użytkownika:
                GUILayout.BeginHorizontal();
                {
                    GUI.backgroundColor = colorBg1;
                    GUI.contentColor = colorText1;
                    //
                    EditorGUI.BeginChangeCheck();
                    _showClosed = GUILayout.Toggle( _showClosed , STR_show_closed );
                    if( EditorGUI.EndChangeCheck() )
                    {
                        SortEntries();
                    }

                    GUILayout.FlexibleSpace();

                    //
                    if( GUILayout.Button( STR_Create_New_Entry , width160 ) )
                    {
                        CreateNewIssueWindow.CreateWindow();
                    }

                    GUILayout.FlexibleSpace();

                    //
                    EditorGUILayout.LabelField( STR_user_name , GUILayout.Width( 70f ) );
                    if( GUILayout.Button( UserNameLoad() , GUILayout.Width( 120f ) ) )
                    {
                        ChangeUserNameWindow.CreateWindow();
                    }

                    //
                    GUI.backgroundColor = Color.white;
                    GUI.contentColor = Color.white;
                }
                GUILayout.EndHorizontal();

                EditorGUILayout.Space();

            }
            catch( System.Exception ex )
            {
                Debug.LogException( ex );
                this.Close();
            }
        }

        #endregion
        #region PRIVATE_METHODS



        #endregion
        #region PUBLIC_METHODS

        [MenuItem( STR_ToDo_slash_Open )]
        public static void CreateWindow ()
        {
            IssueTracker window = EditorWindow.GetWindow<IssueTracker>();
            window.minSize = new Vector2( 100 , 100 );
            window.Show();
        }

        public static string UserNameLoad ()
        {
            return EditorPrefs.GetString( EDITOR_PREF_USER_KEY , STR_Guest );
        }

        public static void UserNameSave ( string newName )
        {
            EditorPrefs.SetString( EDITOR_PREF_USER_KEY , newName );
        }

        public static void EntrySave ( EntryAndFileGUID data )
        {

            //make sure this obj was not disposed:
            data.TestForDisposed();

            //write file:
            System.IO.StreamWriter writer = null;
            try
            {
                string relativePath = AssetDatabase.GUIDToAssetPath( data.guid );
                if( relativePath != null && relativePath.Length != 0 )
                {
                    string absolutePath = GetAbsolutePath( AssetDatabase.GUIDToAssetPath( data.guid ) );
                    if( absolutePath != null && absolutePath.Length != 0 )
                    {
                        writer = new System.IO.StreamWriter( absolutePath );
                        writer.Write( JsonUtility.ToJson( data.entry , true ) );
                    } else
                    {
                        Debug.LogWarning( string.Format( "Unity can't load asset from path\"{0}\"" , relativePath ) );
                    }
                } else
                {
                    Debug.LogWarning( "Unity can't find this asset anymore: \n"+data.ToString() );
                }
            }
            catch( System.UnauthorizedAccessException ex )
            {
                Debug.LogException( ex );
            }
            catch( System.Exception ex )
            {
                Debug.LogException( ex );
            }
            finally
            {
                if( writer != null )
                {
                    writer.Close();
                }
            }

        }

        public static Entry EntryLoad ( string filePathAbsolute )
        {
            try
            {
                if( System.IO.File.Exists( filePathAbsolute ) == true )
                {
                    Entry result = JsonUtility.FromJson<Entry>( System.IO.File.ReadAllText( filePathAbsolute ) );
                    result.UpdatePrevievTexture();
                    return result;
                } else
                {
                    throw new FileNotFoundException( filePathAbsolute+" not found!\n" );
                }
            }
            catch( System.Exception ex )
            {
                Debug.LogException( ex );
                return null;
            }
        }

        public static void RenameTodoFile ( EntryAndFileGUID data )
        {
            AssetDatabase.RenameAsset(
                AssetDatabase.GUIDToAssetPath( data.guid ) ,
                GetEntryFileName( data.entry )+"_"+data.guid
            );
        }

        /// <returns> filename this entry suppose to have </returns>
        public static string GetEntryFileName ( Entry entry )
        {
            var result = new System.Text.StringBuilder();

            result.Append( string.Format( "{0} {1} {2}" ,
                    entry.priority ,
                    entry.state ,
                    entry.text.Replace( ' ' , '_' )
				                //AssetDatabase.AssetPathToGUID( GetRelativePath( filePathAbsolute ) ) 
                )
            );
            //replace all illegal chars
            foreach( char singleChar in Path.GetInvalidFileNameChars() )
            {
                result = result.Replace( singleChar , '_' );
            }
            //trim length, because windows errors:
            if( result.Length > 150 )
            {
                //result = result.Substring( 0 , 200 );
                result.Length = 150;
            }
            //return result:
            return result.ToString();
        }

        public static void EntriesReload ()
        {

            //pozbywamy się starych obiektów:
            foreach( var item in _entries )
            {
                item.Dispose();
            }
            _entries.Clear();

            //tworzymy nową wersję listy:
            foreach( string filePath in System.IO.Directory.GetFiles( Application.dataPath+STR_slash_ToDo_slash ) )
            {
                if( filePath.EndsWith( STR_dot_todo ) )
                {


                    Entry entry = EntryLoad( filePath );

                    string entryGuid = AssetDatabase.AssetPathToGUID( GetRelativePath( filePath ) );
                    if( entryGuid != null && entryGuid.Length != 0 )
                    {
                        EntryAndFileGUID newEntryAndGuid = new EntryAndFileGUID( entryGuid , entry );
                        _entries.Add( newEntryAndGuid );
                        RenameTodoFile( newEntryAndGuid );
                    } else
                    {
                        //AssetPathToGUID failed:
                        Debug.LogWarning( string.Format( "can't get guid for file: \'{0}\'" , filePath ) );
                    }

                }
            }

            //sort:
            SortEntries();

        }

        public static void SortEntries ()
        {
            _entries.Sort(
                ( a , b ) =>
                {
                    int a_assigmentBonus = a.entry.assignedTo != UserNameLoad() ? 0 : 100;
                    int b_assigmentBonus = b.entry.assignedTo != UserNameLoad() ? 0 : 100;
                    int result = -a.entry.CompareTo( b.entry )-( a_assigmentBonus-b_assigmentBonus );
                    //Debug.Log( string.Format( "{0} ({1}) - {2} ({3}) = {4}" , a.Value.CalculateListImportance() , a.Value.sortingScore , b.Value.CalculateListImportance() , b.Value.sortingScore , -result ) );
                    return result;
                }
            );
        }

        public static bool FilterEntryByString ( Entry argEntry , string argFilterPhrase )
        {
            //prepare strings:
            string entryText = argEntry.text.ToUpper();
            string entryType = argEntry.type.ToString().ToUpper();
            string entryPriority = argEntry.priority.ToString().ToUpper();
            string entryState = argEntry.state.ToString().ToUpper();
            string entryObject = argEntry.subject != null ? argEntry.subject.name.ToUpper() : "NULL";
            string[] filterAsWordsArray = argFilterPhrase.Split( ' ' );

            //'-' before word is a way to hide any entry where this word can be found:
            //asses anyWordStartWitheMinus:
            bool anyWordStartWitheMinus = false;
            foreach( var item in filterAsWordsArray )
            {
                if( item.Length > 0 && item[ 0 ] == '-' )
                {
                    anyWordStartWitheMinus = true;
                    break;
                }
            }

            //handle common and simpler case where no word starts with '-':
            if( anyWordStartWitheMinus == false )
            {
                //
                for( int i = 0 ; i < filterAsWordsArray.Length ; i++ )
                {
                    string filterWord = filterAsWordsArray[ i ].ToUpper();
                    if( filterWord.Length > 0 )
                    {
                        if( entryText.Contains( filterWord ) || entryType.Contains( filterWord ) || entryPriority.Contains( filterWord ) || entryState.Contains( filterWord ) || entryObject.Contains( filterWord ) )
                        {
                            //match found:
                            return true;
                        }
                    }
                }
                //no single match found:
                return false;
            }
			//handle slightly more complex case, where at least one word starts with '-' sign:
			else
            {
                bool result = false;
                for( int i = 0 ; i < filterAsWordsArray.Length ; i++ )
                {
                    string filterWord = filterAsWordsArray[ i ].ToUpper();
                    bool thisWordStartsWithMinus = false;
                    if( filterWord.Length > 0 )
                    {
                        thisWordStartsWithMinus = filterWord[ 0 ] == '-';
                        if( thisWordStartsWithMinus == true )
                        {
                            filterWord = filterWord.Substring( 1 );
                        }
                    }
                    if( filterWord.Length > 0 )
                    {
                        if( entryText.Contains( filterWord ) || entryType.Contains( filterWord ) || entryPriority.Contains( filterWord ) || entryState.Contains( filterWord ) || entryObject.Contains( filterWord ) )
                        {
                            if( thisWordStartsWithMinus == false )
                            {
                                result = true;
                            } else
                            {
                                return false;
                            }
                        }
                    }
                }
                //
                return result;
            }
        }

        /// <summary> Converts absolute path to relative path (relative to Assets folder) </summary>
        public static string GetRelativePath ( string filePathAbsolute )
        {
            return filePathAbsolute.Substring( filePathAbsolute.LastIndexOf( STR_Assets_slash ) );
        }

        public static string GetAbsolutePath ( string filePathRelative )
        {
            return Application.dataPath+filePathRelative.TrimStart( 'A' , 's' , 'e' , 't' );
        }

        public static void GuiDrawSeparator_A ( Color argColor )
        {
            GUI.color = argColor;//Color.white;//colorText1*3f;//Color.grey*0.5f;
            GUILayout.Button( GUIContent.none , GUILayout.ExpandWidth( true ) , GUILayout.Height( 1f ) );
            GUI.color = Color.white;
        }
        public static void GuiDrawSeparator_B ( Color argColor )
        {
            GUI.color = argColor;
            GUILayout.Box( GUIContent.none , GUILayout.ExpandWidth( true ) , GUILayout.Height( 1f ) );
            GUI.color = Color.white;
        }

        #endregion
        #region embeded_classes

        [System.Serializable]
        public class EntryAndFileGUID : System.IDisposable
        {
            #region fields_&_properties

            [SerializeField] string _guid;
            public string guid
            {
                get
                {
                    TestForDisposed();
                    return _guid;
                }
                set
                {
                    TestForDisposed();
                    _guid = value;
                }
            }

            [SerializeField] Entry _entry;
            public Entry entry
            {
                get
                {
                    TestForDisposed();
                    return _entry;
                }
                set
                {
                    TestForDisposed();
                    _entry = value;
                }
            }

            bool _disposed = false;
            public bool disposed { get { return _disposed; } }
            //string _stackTraceOnDispose;

            #endregion
            #region constructors

            public EntryAndFileGUID ( string guid , Entry entry )
            {
                this.guid = guid;
                this.entry = entry;
            }

            #endregion
            #region methods

            public override string ToString ()
            {
                return string.Format( "[ guid: {0} , {1} ]" , guid , entry );
            }

            #endregion
            #region IDisposable implementation

            public void TestForDisposed ()
            {
                if( _disposed == true )
                {
                    throw new System.ObjectDisposedException( GetType().ToString() );
                    //throw new System.ObjectDisposedException( string.Format( "{0} was disposed with stack trace: \'{1}\'" , GetType() , _stackTraceOnDispose ) );
                }
            }

            /// <summary> Release all resource used by the object. </summary>
            public void Dispose ()
            {

                //read stack trace:
                //_stackTraceOnDispose = System.Environment.StackTrace;

                this._guid = null;
                this._entry = null;

                //flag as disposed:
                _disposed = true;

            }

            #endregion
        }

        #endregion
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    class CreateNewIssueWindow : EditorWindow
    {

        static Entry _new_entry;

        void OnEnable ()
        {
            this.titleContent = new GUIContent( IssueTracker.STR_New_Issue , IssueTracker.instance.icon_window );
        }

        void OnLostFocus ()
        {
            this.Close();
        }

        void OnGUI ()
        {

            if( _new_entry == null )
            {
                _new_entry = new Entry(
                    IssueTracker.STR_message ,
                    Entry.Type.task ,
                    Entry.Priority.normal ,
                    Entry.State.open ,
                    IssueTracker.UserNameLoad() ,
                    null
                );
            }

            GUI.backgroundColor = _new_entry.CalculateColor();

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            {
                //przycisk dodawanie nowego wpisu:
                if( GUILayout.Button( IssueTracker.STR_Create , IssueTracker.width80 , GUILayout.ExpandHeight( true ) ) )
                {
					
                    //make sure author name is correct:
                    _new_entry.createdByUser = IssueTracker.UserNameLoad();

                    //zapisz wpisz do pliku (# musi poprzedzać hashcode):
                    System.IO.StreamWriter writer = null;
                    try
                    {
                        string filePath = Application.dataPath+IssueTracker.STR_slash_ToDo_slash+IssueTracker.GetEntryFileName( _new_entry )+IssueTracker.STR_dot_todo;
                        writer = new System.IO.StreamWriter( filePath );
                        writer.Write( JsonUtility.ToJson( _new_entry ) );
                    }
                    catch( System.Exception ex )
                    {
                        throw ex;
                    }
                    finally
                    {
                        if( writer != null )
                        {
                            writer.Close();
                        }
                    }

                    //make Editor import this just created file (otherwise editor wont be able to load this entry's guid):
                    AssetDatabase.Refresh();

                    //call reload entries:
                    IssueTracker.EntriesReload();

                    //clear entry:
                    _new_entry = null;

                    //zamknij okno:
                    this.Close();
                    return;
                }

                GUILayout.BeginVertical();
                {
                    //GUILayout.BeginHorizontal();
                    //{
                    //treść dla nowego wpisu:
                    _new_entry.text = EditorGUILayout.TextArea( _new_entry.text , IssueTracker.heightX3 , GUILayout.MaxWidth( 407f ) );
                    //}
                    //GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        _new_entry.type = (Entry.Type)EditorGUILayout.EnumPopup( (System.Enum)_new_entry.type , IssueTracker.width80 );
                        _new_entry.priority = (Entry.Priority)EditorGUILayout.EnumPopup( (System.Enum)_new_entry.priority , IssueTracker.width80 );
                        _new_entry.state = (Entry.State)EditorGUILayout.EnumPopup( (System.Enum)_new_entry.state , IssueTracker.width80 );
                        //_new_entry.subject = EditorGUILayout.ObjectField( _new_entry.subject , typeof(Object) , false );
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();

            }
            GUILayout.EndHorizontal();

            //GUILayout.FlexibleSpace();
        }

        public static void CreateWindow ()
        {
            CreateNewIssueWindow window = EditorWindow.GetWindow<CreateNewIssueWindow>();
            window.hideFlags = HideFlags.DontSave;
            window.minSize = new Vector2( 500f , 70f );
            window.maxSize = new Vector2( 501f , 71f );
            window.Show();
        }

    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    class ChangeUserNameWindow : EditorWindow
    {

        string _newUserName;

        void OnEnable ()
        {
            this.titleContent = new GUIContent( IssueTracker.STR_change_user_name , IssueTracker.instance.icon_window );
            _newUserName = IssueTracker.UserNameLoad();
        }

        void OnLostFocus ()
        {
            this.Close();
        }

        void OnGUI ()
        {
            GUI.backgroundColor = Color.red;
            GUI.contentColor = IssueTracker.colorText1;

            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            _newUserName = EditorGUILayout.TextField( _newUserName );
            if( GUILayout.Button( IssueTracker.STR_apply , GUILayout.ExpandWidth( false ) ) )
            {
                IssueTracker.UserNameSave( _newUserName );
                IssueTracker.EntriesReload();
                this.Close();
            }
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
        }

        public static void CreateWindow ()
        {
            ChangeUserNameWindow window = EditorWindow.GetWindow<ChangeUserNameWindow>();
            window.hideFlags = HideFlags.DontSave;
            window.minSize = new Vector2( 300f , 40f );
            window.maxSize = new Vector2( 301f , 41f );
            window.Show();
        }

    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    class EntryDetailsWindow : EditorWindow
    {
        #region fields

        IssueTracker.EntryAndFileGUID _selected;

        #endregion
        #region editorwindow_methods

        void OnEnable ()
        {

            //does main window exists?
            if( IssueTracker.instance == null )
            {
                IssueTracker.CreateWindow();
            }

            //set title:
            this.titleContent = new GUIContent( IssueTracker.STR_entry_details , IssueTracker.instance.icon_window );

        }
        
        /*void OnDisable () {

			//dispose _selected when not found in main entries list:
			if( _selected!=null ) {
				bool exeDispose = true;
				foreach( var item in IssueTracker._entries ) {
					if( item.guid==_selected.guid ) {
						exeDispose = false;
						break;
					}
				}
				if( exeDispose==true ) {
					_selected.Dispose();
				}
			}

			//null fields:
			_selected = null;

			//force files to be renamed & reloaded after edit:
			IssueTracker.EntriesReload();

		}*/
		
        void OnLostFocus ()
        {
            this.Close();
        }

        void OnGUI ()
        {

            if( _selected != null && _selected.entry != null )
            {

                GUI.backgroundColor = new Color( 0.4f , 0.3f , 0.2f );
                GUI.contentColor = IssueTracker.colorText1;

                GUILayout.FlexibleSpace();

                EditorGUI.BeginChangeCheck();
                {
                    //pole tekstowe wpisu:
                    _selected.entry.text = EditorGUILayout.TextArea( _selected.entry.text , GUILayout.MinHeight( IssueTracker.singleLineHeight*3f ) );
				

                    GUILayout.FlexibleSpace();

                    //inne pola:
                    EditorGUILayout.BeginHorizontal();
                    {
						
                        Color prevColor = GUI.contentColor;
                        GUI.contentColor = IssueTracker.colorText2;

                        GUILayout.FlexibleSpace();
                        _selected.entry.type = (Entry.Type)EditorGUILayout.EnumPopup( (System.Enum)_selected.entry.type , IssueTracker.width80 );
                        GUILayout.FlexibleSpace();
                        _selected.entry.priority = (Entry.Priority)EditorGUILayout.EnumPopup( (System.Enum)_selected.entry.priority , IssueTracker.width80 );
                        GUILayout.FlexibleSpace();
                        _selected.entry.state = (Entry.State)EditorGUILayout.EnumPopup( (System.Enum)_selected.entry.state , IssueTracker.width80 );

                        GUI.contentColor = prevColor;

                        GUILayout.FlexibleSpace();
                        _selected.entry.subject = EditorGUILayout.ObjectField( _selected.entry.subject , typeof(Object) , false , IssueTracker.width80 );
                        GUILayout.FlexibleSpace();

                    }
                    EditorGUILayout.EndHorizontal();


                    //pole autor:
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Label( IssueTracker.STR_created_by );
                        GUILayout.Label( _selected.entry.createdByUser );
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    {
                        //przycisk kasowania wpisu:
                        GUI.backgroundColor = Color.red;
                        if( GUILayout.Button( IssueTracker.STR_delete_entry , IssueTracker.width80 ) )
                        {
                            if( EditorUtility.DisplayDialog(
                                    IssueTracker.STR_delete_entry ,
                                    string.Format( "Delete?\n{0} ?" , _selected.entry.title ) ,
                                    IssueTracker.STR_yes ,
                                    IssueTracker.STR_no
                                ) )
                            {

                                //delete this entry:
                                if( AssetDatabase.MoveAssetToTrash( AssetDatabase.GUIDToAssetPath( _selected.guid ) ) )
                                {

                                    //release selected (deleted) entry:
                                    _selected = null;

                                    //refresh Project window:
                                    AssetDatabase.Refresh();

                                    //reload entries in main window:
                                    IssueTracker.EntriesReload();

                                    //close this window:
                                    this.Close();
                                }

                            }
                        }
                        GUI.backgroundColor = Color.white;

                        GUILayout.FlexibleSpace();

                        GUI.backgroundColor = Color.grey;
                        if( GUILayout.Button( IssueTracker.STR_close , IssueTracker.width80 ) )
                        {
                            this.Close();
                        }
                        GUI.backgroundColor = Color.white;
                    }
                    EditorGUILayout.EndHorizontal();

                }
                if( EditorGUI.EndChangeCheck() && _selected != null )
                {
                    IssueTracker.EntrySave( _selected );
                    IssueTracker.EntryAndFileGUID getObj = IssueTracker._entries.Find( (obj ) =>
                        {
                            return obj.guid == _selected.guid;
                        } );
                    getObj.entry = _selected.entry;
                    if( getObj.guid != _selected.guid )
                    {
                        Debug.LogWarning( "getObj.guid!=selected.guid" );
                    }
                    IssueTracker.instance.Repaint();
                }

            } else
            {

                GUI.backgroundColor = new Color( 0.4f , 0.3f , 0.2f );
                GUI.contentColor = IssueTracker.colorText1;

                GUILayout.FlexibleSpace();
                GUILayout.Label( IssueTracker.STR_select_entry_to_see_details );
                GUILayout.FlexibleSpace();

            }
        }

        #endregion
        #region public_methods

        public static void CreateWindow ( IssueTracker.EntryAndFileGUID data )
        {
            EntryDetailsWindow window = EditorWindow.GetWindow<EntryDetailsWindow>();
            window.hideFlags = HideFlags.DontSave;
            window.minSize = new Vector2( 500f , 120f );
            window.maxSize = new Vector2( 501f , 121f );
            window.Show();
            //
            window._selected = data;
            //
        }

        //przy próbie otwarcia jakiegoś pliku w unity sprawdzamy czy to jest nasz plik .todo, jeśli jest otwieramy go sami:
        [OnOpenAssetAttribute( 1 )]
        public static bool OnOpenAsset_TodoFile ( int instanceID , int line )
        {
            try
            {
                string getPath = AssetDatabase.GetAssetPath( instanceID );
                string getGuid = AssetDatabase.AssetPathToGUID( getPath );
                if( getPath.EndsWith( IssueTracker.STR_dot_todo ) == true )
                {
                    CreateWindow(
                        IssueTracker.instance != null ?
						IssueTracker._entries.Find( (obj ) => obj.guid == getGuid ) :
						new IssueTracker.EntryAndFileGUID(
                            getGuid ,
                            IssueTracker.EntryLoad( getPath )
                        )
                    );
                    return true;
                }
                return false;
            }
            catch( System.Exception ex )
            {
                Debug.LogException( ex );
                return false;
            }
        }

        #endregion
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [System.Serializable]
    class Entry : ISerializationCallbackReceiver, System.IDisposable, System.IComparable<Entry>
    {
        #region fields

        public string text = "undefined";

        public Type type = Type.note;
        public Priority priority = Priority.backlog;
        public State state = State.open;

        public int sortingScore = 0;
        public string assignedTo = IssueTracker.STR_no_one;

        public string createdByUser = "unknown";

        public int fileVersion = 0;

        [System.NonSerialized] public Object subject;
        [SerializeField] string _subject;

        [System.NonSerialized] public Texture2D subjectPreview;

        public string title
        {
            get
            {
                if( text.GetHashCode() != text_lastHashCode )
                {
                    //get hash for change detection:
                    this.text_lastHashCode = this.text.GetHashCode();
                    //prepare new title:
                    string[] strings = text.Split( '\n' );
                    if( strings.Length < 2 )
                    {
                        title_cached = this.text;
                    } else
                    {
                        title_cached = string.Format( "{0}\n{1}" , strings[ 0 ] , strings[ 1 ] );
                    }
                }
                return title_cached;
            }
        }
        [System.NonSerialized] string title_cached;
        [System.NonSerialized] int text_lastHashCode;

        bool _disposed = false;
        public bool disposed { get { return _disposed; } }
        //string _stackTraceOnDispose;

        #endregion
        #region constructors

        public Entry ( string setText , Type setType , Priority setPriority , State setState , string setAuthor , Object pointThisObject )
        {
            this.text = setText;
            this.type = setType;
            this.priority = setPriority;
            this.state = setState;
            this.createdByUser = setAuthor;
            this.subject = pointThisObject;
        }

        #endregion
        #region public_methods

        public void UpdatePrevievTexture ()
        {
            if( this.subject != null )
            {
                Object getPersistentObject = AssetDatabase.LoadMainAssetAtPath( AssetDatabase.GetAssetPath( this.subject ) );
                this.subjectPreview = AssetPreview.GetAssetPreview( getPersistentObject );
                if( this.subjectPreview == null )
                {
                    this.subjectPreview = AssetPreview.GetMiniThumbnail( getPersistentObject );
                }
            }
        }

        public override string ToString ()
        {
            return string.Format(
                "[ text: {0} , type: {1} , priority: {2} , state: {3} , createdByUser: {4} , subject: {5} , title_cached: {6} ]" ,
                text ,
                type ,
                priority ,
                state ,
                createdByUser ,
                subject ,
                title_cached
            );
        }

        #endregion
        #region ISerializationCallbackReceiver implementation

        public void OnBeforeSerialize ()
        {

            TestForDisposed();

            if( this.subject != null )
            {
                this._subject = AssetDatabase.AssetPathToGUID( AssetDatabase.GetAssetPath( this.subject ) );
            } else
            {
                this._subject = null;
            }
        }

        public void OnAfterDeserialize ()
        {
            if( this._subject != null )
            {
                this.subject = AssetDatabase.LoadMainAssetAtPath( AssetDatabase.GUIDToAssetPath( this._subject ) );
            }
        }

        #endregion
        #region IComparable implementation

        public int CompareTo ( Entry other )
        {
            if( other == null )
            {
                throw new System.NullReferenceException();
            }
            return Mathf.RoundToInt( ( this.CalculateListImportance()-( other as Entry ).CalculateListImportance() )*10f );
        }

        /// <summary> Scores the list entry imporance to user </summary>
        /// <returns> 0f-1f </returns>
        public float CalculateListImportance ()
        {
            //scores:
            Dictionary<Priority,int> listImportanceScore_priority = new Dictionary<Priority, int>()
            {
                {
                    Priority.backlog ,
                    0
                },
                {
                    Priority.normal ,
                    3
                },
                {
                    Priority.important ,
                    4
                },
                {
                    Priority.urgent ,
                    6
                },
            };
            Dictionary<State,int> listImportanceScore_state = new Dictionary<State, int>()
            {
                {
                    State.canceled ,
                    -1
                },
                {
                    State.done ,
                    0
                },
                {
                    State.open ,
                    1
                },
            };
            Dictionary<Type,int> listImportanceScore_type = new Dictionary<Type, int>()
            {
                {
                    Type.feedback ,
                    -1
                },
                {
                    Type.note ,
                    0
                },
                { Type.task , 1 },
				{ Type.bug , 2 },
			};
			// calculate highiest score possible:
			int highiestScorePossible = 0;
			{
				int maxForPriority = int.MinValue;
				foreach( var val in listImportanceScore_priority.Values ) {
					maxForPriority = val>maxForPriority ? val : maxForPriority;
				}
				int maxForState = int.MinValue;
				foreach( var val in listImportanceScore_state.Values ) {
					maxForState = val>maxForState ? val : maxForState;
				}
				int maxForType = int.MinValue;
				foreach( var val in listImportanceScore_type.Values ) {
					maxForType = val>maxForType ? val : maxForType;
				}
				highiestScorePossible = maxForPriority+maxForState+maxForType;
			}
			//calculate result:
			float result = Mathf.InverseLerp( 0f , (float)highiestScorePossible , (float)(listImportanceScore_priority[this.priority]+listImportanceScore_state[this.state]+listImportanceScore_type[this.type]) );
			this.sortingScore = Mathf.RoundToInt( result*10f );
			return result;
		}

		public Color CalculateColor () {
			float c = this.CalculateListImportance();
			c *= c;
			return new Color( c , 0.5f-0.5f*c , 0.5f-0.5f*c , 0.7f );
		}

		#endregion
		#region IDisposable implementation

		public void TestForDisposed () {
			if( _disposed==true ) {
				throw new System.ObjectDisposedException( GetType().ToString() );
				//throw new System.ObjectDisposedException( string.Format( "{0} was disposed with stack trace: \'{1}\'" , GetType() , _stackTraceOnDispose ) );
			}
		}

		public void Dispose () {

			//read stack trace:
			//_stackTraceOnDispose = System.Environment.StackTrace;

			//
			this.text = null;
			this.createdByUser = null;
			this._subject = null;
			this.subject = null;
			this.subjectPreview = null;
			this.assignedTo = null;
			this.createdByUser = null;
			this.title_cached = null;

			//flag as disposed:
			_disposed = true;

		}

		#endregion

		public enum Type {
			note,
			bug,
			task,
			feedback,
		}

		public enum Priority {
			backlog,
			normal,
			important,
			urgent,
		}

		public enum State {
			open,
			done,
			canceled,
		}

	}

}
