using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Gw2Launcher
{
    public static class Settings
    {
        private const ushort VERSION = 10;

        public const string ASSET_HOST = "assetcdn.101.arenanetworks.com";
        public const string ASSET_COOKIE = "authCookie=access=/latest/*!/manifest/program/*!/program/*~md5=4e51ad868f87201ad93e428ff30c6691";

        private const ushort WRITE_DELAY = 10000;
        private const string FILE_NAME = "settings.dat";
        private static readonly byte[] HEADER;
        private static readonly Type[] FORMS;

        #region Enums

        public enum ImagePlacement : byte
        {
            /// <summary>
            /// Image will shift other elements if needed
            /// </summary>
            Shift = 0,
            /// <summary>
            /// Image will overflow into the background if needed
            /// </summary>
            Overflow = 1,
        }

        [Flags]
        public enum RelaunchOptions : byte
        {
            None = 0,
            Exit = 1,
            Relaunch = 2,
        }

        [Flags]
        public enum JumpListOptions : byte
        {
            None = 0,
            Enabled = 1,
            OnlyShowInactive = 2,
            OnlyShowDaily = 4,
        }

        public enum AccountType : byte
        {
            GuildWars2 = 0,
            GuildWars1 = 1,
        }

        public enum SortMode : byte
        {
            None = 0,
            Name = 1,
            Account = 2,
            LastUsed = 3,
            LaunchTime = 4,
            CustomList = 5,
            CustomGrid = 6,
        }

        public enum SortOrder : byte
        {
            Ascending = 0,
            Descending = 1
        }

        [Flags]
        public enum GroupMode : byte
        {
            None = 0,
            Active = 1,
            Type = 2,

            //Descending = 128,
        }

        public enum ScreenAnchor : byte
        {
            Top = 0,
            Bottom = 1,
            Left = 2,
            Right = 3,
            TopLeft = 4,
            TopRight = 5,
            BottomLeft = 6,
            BottomRight = 7,
            None = 8,
        }

        public enum ButtonAction : byte
        {
            None = 0,
            Focus = 1,
            Close = 2,
            Launch = 3,
            LaunchSingle = 4,
            FocusAndCopyAuthenticator = 5,
            CopyAuthenticator = 6,
            ShowAuthenticator = 7,
        }

        public enum ScreenshotFormat : byte
        {
            Default = 0,
            Bitmap = 1
        }

        [Flags]
        public enum DailiesMode : byte
        {
            None = 0,
            Show = 1,
            Positioned = 2,
            AutoLoad = 4,
        }

        [Flags]
        public enum MuteOptions : byte
        {
            None = 0,
            All = 1,
            Music = 2,
            Voices = 4,
        }

        public enum NetworkAuthorizationState : byte
        {
            Disabled = 0,
            Unknown = 1,
            OK = 2,
        }

        [Flags]
        public enum NetworkAuthorizationFlags : byte
        {
            None = 0,
            Manual = 1,
            Automatic = 2,
            Always = 4,

            VerificationModes = 7,

            VerifyIP = 16,
            RemoveAll = 32,
            AbortLaunchingOnFail = 64,
            RemovePreviouslyAuthorized = 128,
        }

        public enum ApiCacheState : byte
        {
            None = 0,
            OK = 1,
            Pending = 2,
        }

        public enum ProcessPriorityClass : byte
        {
            None = 0,
            High = 1,
            AboveNormal = 2,
            Normal = 3,
            BelowNormal = 4,
            Low = 5,
        }

        public static System.Diagnostics.ProcessPriorityClass ToProcessPriorityClass(this ProcessPriorityClass priority)
        {
            switch (priority)
            {
                case Settings.ProcessPriorityClass.High:
                    return System.Diagnostics.ProcessPriorityClass.High;
                case Settings.ProcessPriorityClass.AboveNormal:
                    return System.Diagnostics.ProcessPriorityClass.AboveNormal;
                case Settings.ProcessPriorityClass.BelowNormal:
                    return System.Diagnostics.ProcessPriorityClass.BelowNormal;
                case Settings.ProcessPriorityClass.Low:
                    return System.Diagnostics.ProcessPriorityClass.Idle;
            }
            return System.Diagnostics.ProcessPriorityClass.Normal;
        }

        [Flags]
        public enum WindowOptions : byte
        {
            None = 0,
            Windowed = 1,
            RememberChanges = 2,
            PreventChanges = 4,
            TopMost = 8,
            DisableTitleBarButtons = 16,
        }

        [Flags]
        public enum AutologinOptions : byte
        {
            None = 0,
            Login = 1,
            Play = 2,
        }

        public enum IconType : byte
        {
            None = 0,
            File = 1,
            Gw2LauncherColorKey = 2,
            ColorKey = 3,
        }

        public enum JumpListPinning : byte
        {
            None = 0,
            Disabled = 1,
        }

        [Flags]
        public enum AccountBarStyles : byte
        {
            None = 0,
            Name = 1,
            Color = 2,
            Icon = 4,
            Exit = 8,
            HighlightFocused = 16,
        }

        [Flags]
        public enum AccountBarOptions : byte
        {
            None = 0,
            HorizontalLayout = 1,
            OBSOLETE_GroupByActive = 2,
            OnlyShowInactive = 2,
            OnlyShowActive = 4,
            AutoHide = 8,
            TopMost = 16,
            HideGw1 = 32,
            HideGw2 = 64,
        }

        [Flags]
        public enum LocalizeAccountExecutionOptions : byte
        {
            None = 0,
            Enabled = 1,
            ExcludeUnknownFiles = 2,
            OnlyIncludeBinFolders = 4,
            AutoSync = 8,
            AutoSyncDeleteUnknowns = 16,
        }

        public enum EncryptionScope : byte
        {
            CurrentUser = 0,
            LocalMachine = 1,
            Portable = 2,
            Unencrypted = 3,
        }

        public enum ProfileMode : byte
        {
            Advanced = 0,
            Basic = 1
        }

        [Flags]
        public enum PatchingFlags : byte
        {
            None = 0,
            UseHttps = 1,
            OverrideHosts = 2,
        }

        [Flags]
        public enum ProfileModeOptions
        {
            None = 0,
            RestoreOriginalPath = 1,
            ClearTemporaryFiles = 2,
        }

        #endregion

        #region Classes/Interfaces

        public class ImageOptions
        {
            public ImageOptions(string path, ImagePlacement placement)
            {
                this.Path = path;
                this.Placement = placement;
            }

            public string Path
            {
                get;
                private set;
            }

            public ImagePlacement Placement
            {
                get;
                private set;
            }
        }

        public class PageData
        {
            public PageData(byte page, ushort sortKey)
            {
                this.Page = page;
                this.SortKey = sortKey;
            }

            public byte Page
            {
                get;
                set;
            }

            public ushort SortKey
            {
                get;
                set;
            }
        }

        public class LaunchLimiterOptions
        {
            public LaunchLimiterOptions()
            {

            }

            public LaunchLimiterOptions(byte count, byte recharge, byte time)
            {
                this.Count = count;
                this.RechargeCount = recharge;
                this.RechargeTime = time;
            }

            public byte Count
            {
                get;
                private set;
            }

            public byte RechargeCount
            {
                get;
                private set;
            }

            public byte RechargeTime
            {
                get;
                private set;
            }

            public bool IsAutomatic
            {
                get
                {
                    return Count == 0;
                }
            }
        }

        public class WindowTemplate
        {
            public class Screen
            {
                public Screen(Rectangle bounds, Rectangle[] windows)
                {
                    this.Bounds = bounds;
                    this.Windows = windows;
                }

                public Rectangle Bounds
                {
                    get;
                    protected set;
                }

                public Rectangle[] Windows
                {
                    get;
                    protected set;
                }
            }

            public WindowTemplate(Screen[] screens)
            {
                Screens = screens;
            }

            public Screen[] Screens
            {
                get;
                protected set;
            }
        }

        public class AccountGridButtonColors : IEquatable<AccountGridButtonColors>
        {
            public enum Colors
            {
                Name,
                User,
                StatusDefault, StatusError, StatusOK, StatusWaiting,
                BackColorDefault, BackColorHovered, BackColorSelected,
                ForeColorHovered, ForeColorSelected,
                BorderLightDefault, BorderLightHovered, BorderLightSelected,
                BorderDarkDefault, BorderDarkHovered, BorderDarkSelected,
                FocusedHighlight, FocusedBorder,
                ActionExitFill, ActionExitFlash,
                ActionFocusFlash
            }

            public AccountGridButtonColors()
            {
                Values = new Color[Enum.GetValues(typeof(Colors)).Length];
            }

            public Color[] Values;

            public Color this[Colors c]
            {
                get
                {
                    return Values[(int)c];
                }
                set
                {
                    Values[(int)c] = value;
                }
            }

            public bool Equals(AccountGridButtonColors o)
            {
                for (var i = Values.Length - 1; i >= 0; --i )
                {
                    if (Values[i].ToArgb() != o.Values[i].ToArgb())
                        return false;
                }

                return true;
            }

            public AccountGridButtonColors Clone()
            {
                var colors = new AccountGridButtonColors();
                Array.Copy(Values, colors.Values, Values.Length);
                return colors;
            }
        }

        public struct SortingOptions : IEquatable<SortingOptions>
        {
            public const byte ARRAY_SIZE = 2;

            public struct SortingMode
            {
                public SortingMode(SortMode mode, bool descending)
                {
                    this.Mode = mode;
                    this.Descending = descending;
                }

                public SortMode Mode;
                public bool Descending;
            }

            public struct GroupingMode
            {
                public GroupingMode(GroupMode mode, bool descending)
                {
                    this.Mode = mode;
                    this.Descending = descending;
                }

                public GroupMode Mode;
                public bool Descending;

                public bool HasFlag(GroupMode m)
                {
                    return (Mode & m) == m;
                }
            }

            public SortingMode Sorting;
            public GroupingMode Grouping;

            public SortingOptions(SortMode sorting, bool sortDescending, GroupMode grouping, bool groupDescending)
            {
                Sorting = new SortingMode(sorting,sortDescending);
                Grouping = new GroupingMode(grouping, groupDescending);
            }

            public SortingOptions(SortMode sorting, bool sortDescending, GroupingMode grouping)
            {
                Sorting = new SortingMode(sorting, sortDescending);
                Grouping = grouping;
            }

            public SortingOptions(SortingMode sorting, GroupMode grouping, bool groupDescending)
            {
                Sorting = sorting;
                Grouping = new GroupingMode(grouping, groupDescending);
            }

            public SortingOptions(byte[] b)
            {
                Sorting = new SortingMode((SortMode)(b[0] & ~128), (b[0] & 128) == 128);
                Grouping = new GroupingMode((GroupMode)(b[1] & ~128), (b[1] & 128) == 128);
            }

            public bool Equals(SortingOptions o)
            {
                return Sorting.Mode == o.Sorting.Mode && Sorting.Descending == o.Sorting.Descending
                    && Grouping.Mode == o.Grouping.Mode && Grouping.Descending == o.Grouping.Descending;
            }

            public bool IsEmpty
            {
                get
                {
                    return Sorting.Mode == SortMode.None && !Sorting.Descending 
                        && Grouping.Mode == GroupMode.None && !Grouping.Descending;
                }
            }

            public byte[] ToBytes()
            {
                return new byte[] 
                { 
                    (byte)(Sorting.Mode | (Sorting.Descending ? (SortMode)128 : (SortMode)0)), 
                    (byte)(Grouping.Mode | (Grouping.Descending ? (GroupMode)128 : (GroupMode)0)) 
                };
            }
        }

        public abstract class PasswordString : IDisposable
        {
            public static PasswordString Create(System.Security.SecureString s)
            {
                return new ProtectedString(s);
            }

            public Security.Cryptography.ProtectedString Data
            {
                get;
                protected set;
            }

            public ushort UID
            {
                get;
                protected set;
            }

            public System.Security.SecureString ToSecureString()
            {
                return Data.ToSecureString();
            }

            public void Dispose()
            {
                Data.Dispose();
            }
        }

        private class ProtectedString : PasswordString
        {
            public ProtectedString(Security.Cryptography.Crypto crypto, byte[] data)
            {
                this.Data = new Security.Cryptography.ProtectedString(crypto, data);
                this.UID = GetUID();
            }

            public ProtectedString(Security.Cryptography.Crypto crypto, byte[] data, ushort uid)
            {
                this.Data = new Security.Cryptography.ProtectedString(crypto, data);
                if (uid == 0)
                    uid = GetUID();
                this.UID = uid;
            }

            public ProtectedString(System.Security.SecureString s)
            {
                this.Data = new Security.Cryptography.ProtectedString(s);
                this.UID = GetUID();
            }

            private ushort GetUID()
            {
                var nid = 0;

                lock (_Accounts)
                {
                    foreach (var uid in _Accounts.Keys)
                    {
                        var a = (Account)_Accounts[uid].Value;
                        if (a != null)
                        {
                            var p = a._Password;
                            if (p != null && p.UID > nid)
                                nid = p.UID;
                        }
                    }
                }

                return (ushort)(nid + 1);
            }
        }

        public class EncryptionOptions
        {
            public EncryptionOptions(EncryptionScope scope, byte[] key)
            {
                this.Scope = scope;
                this.Key = key;
            }

            public EncryptionScope Scope
            {
                get;
                private set;
            }

            public byte[] Key
            {
                get;
                private set;
            }
        }

        public class Notes : IEnumerable<Notes.Note>
        {
            public class Note : IComparable<Note>, IComparable<DateTime>
            {
                private DateTime expires;
                private ushort sid;
                private bool notify;

                public Note(DateTime expires, ushort sid, bool notify)
                {
                    this.expires = expires;
                    this.sid = sid;
                    this.notify = notify;
                }

                public Note(DateTime expires)
                {
                    this.expires = expires;
                }

                public DateTime Expires
                {
                    get
                    {
                        return expires;
                    }
                    private set
                    {
                        expires = value;
                    }
                }

                public ushort SID
                {
                    get
                    {
                        return sid;
                    }
                    set
                    {
                        if (sid != value)
                        {
                            sid = value;
                            OnValueChanged();
                        }
                    }
                }

                public bool Notify
                {
                    get
                    {
                        return notify;
                    }
                    set
                    {
                        if (notify != value)
                        {
                            notify = value;
                            OnValueChanged();
                        }
                    }
                }

                public int CompareTo(Note other)
                {
                    var c = this.Expires.CompareTo(other.Expires);
                    if (c == 0)
                        c = this.SID.CompareTo(other.SID);
                    return c;
                }

                public int CompareTo(DateTime other)
                {
                    return this.Expires.CompareTo(other);
                }

                public Note Clone()
                {
                    return new Note(expires, sid, notify);
                }
            }

            public event EventHandler<Note> Added, Removed;

            private Note[] notes;
            private ushort count;

            public Notes(int capacity)
            {
                if (capacity > 0)
                    notes = new Note[capacity];
            }

            /// <summary>
            /// The passed array will be used as the initial buffer
            /// </summary>
            /// <param name="notes">A sorted array of notes</param>
            public Notes(Note[] notes)
            {
                this.notes = notes;
                count = (ushort)notes.Length;
            }

            public Notes()
            {

            }

            public void Add(Note note)
            {
                lock (this)
                {
                    int index;

                    if (count == 0)
                    {
                        if (notes == null)
                            notes = new Note[3];
                        index = 0;
                    }
                    else
                    {
                        index = Array.BinarySearch<Note>(notes, 0, count, note);
                        if (index < 0)
                            index = ~index;

                        if (count == notes.Length)
                        {
                            var copy = new Note[count + 5];
                            Array.Copy(notes, 0, copy, 0, index);
                            if (index < count)
                                Array.Copy(notes, index, copy, index + 1, count - index);
                            notes = copy;
                        }
                        else if (index < count)
                        {
                            Array.Copy(notes, index, notes, index + 1, count - index);
                        }
                    }

                    notes[index] = note;
                    count++;
                }

                OnValueChanged();

                if (Added != null)
                    Added(this, note);
            }

            public void AddRange(Note[] notes)
            {
                lock (this)
                {
                    if (count + notes.Length > this.notes.Length)
                    {
                        var copy = new Note[count + notes.Length];
                        Array.Copy(this.notes, copy, count);
                    }

                    foreach (var n in notes)
                        Add(n);
                }
            }

            public int IndexOf(DateTime date)
            {
                lock (this)
                {
                    if (count == 0)
                        return -1;

                    var index = Array.BinarySearch<Note>(notes, 0, count, new Note(date));
                    if (index < 0)
                        return ~index;
                    return index;
                }
            }

            public int IndexOf(Note note)
            {
                lock (this)
                {
                    if (count == 0)
                        return -1;

                    var index = Array.BinarySearch<Note>(notes, 0, count, note);
                    if (index < 0)
                        return -1;

                    if (object.Equals(notes[index], note))
                        return index;

                    int i = index - 1;
                    Note n;

                    while ((n = notes[i]).CompareTo(note) == 0)
                    {
                        if (object.Equals(n, note))
                            return i;
                        i--;
                    }

                    i = index + 1;

                    while ((n = notes[i]).CompareTo(note) == 0)
                    {
                        if (object.Equals(n, note))
                            return i;
                        i++;
                    }

                    return -1;
                }
            }

            public bool Remove(Note note)
            {
                lock (this)
                {
                    var i = IndexOf(note);
                    if (i == -1)
                        return false;

                    if (i != count - 1)
                    {
                        Array.Copy(notes, i + 1, notes, i, count - i - 1);
                        notes[count - 1] = null;
                    }
                    else if (count == 1)
                    {
                        notes = null;
                    }
                    else
                    {
                        notes[i] = null;
                    }

                    count--;
                }

                OnValueChanged();

                if (Removed != null)
                    Removed(this, note);

                return true;
            }

            public bool RemoveRange(int index, int count)
            {
                Note[] removed;

                lock (this)
                {
                    if (index + count > this.count)
                        return false;

                    if (Removed != null)
                    {
                        removed = new Note[count];
                        Array.Copy(notes, index, removed, 0, count);
                    }
                    else
                        removed = null;

                    if (index + count == this.count)
                    {
                        Array.Clear(notes, index, count);
                    }
                    else
                    {
                        Array.Copy(notes, index + count, notes, index, this.count - index - count);
                        Array.Clear(notes, this.count - count, count);
                    }

                    this.count -= (ushort)count;
                }

                OnValueChanged();

                if (removed != null)
                {
                    foreach (var r in removed)
                    {
                        if (Removed != null)
                            Removed(this, r);
                    }
                }

                return true;
            }

            public void CopyTo(int index, Note[] array, int offset, int count)
            {
                Array.Copy(notes, index, array, offset, count);
            }

            public Note this[int index]
            {
                get
                {
                    return notes[index];
                }
            }

            public DateTime ExpiresLast
            {
                get
                {
                    if (count > 0)
                        return notes[count - 1].Expires;
                    return DateTime.MinValue;
                }
            }

            public DateTime ExpiresFirst
            {
                get
                {
                    if (count > 0)
                        return notes[0].Expires;
                    return DateTime.MinValue;
                }
            }

            public int Count
            {
                get
                {
                    return count;
                }
            }

            public IEnumerator<Note> GetEnumerator()
            {
                for (var i = 0; i < count; i++)
                    yield return notes[i];
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public Notes Clone()
            {
                var a = new Notes();

                if (notes != null)
                {
                    a.notes = new Note[notes.Length];
                    for (var i = 0; i < notes.Length; i++)
                    {
                        var n = notes[i];
                        if (n != null)
                            a.notes[i] = n.Clone();
                    }

                    a.count = (ushort)notes.Length;
                }

                return a;
            }
        }

        public interface IAccountApiData
        {
            IApiValue<ushort> DailyPoints
            {
                get;
                set;
            }
            IApiValue<int> Played
            {
                get;
                set;
            }
            IApiValue<T> CreateValue<T>();
        }

        public interface IApiValue<T>
        {
            DateTime LastChange
            {
                get;
                set;
            }
            T Value
            {
                get;
                set;
            }
            ApiCacheState State
            {
                get;
                set;
            }
        }

        private class ApiValue<T> : IApiValue<T>
        {
            public DateTime _LastChange;
            public DateTime LastChange
            {
                get
                {
                    return _LastChange;
                }
                set
                {
                    if (_LastChange != value)
                    {
                        _LastChange = value;
                        OnValueChanged();
                    }
                }
            }

            public T _Value;
            public T Value
            {
                get
                {
                    return _Value;
                }
                set
                {
                    if (!_Value.Equals(value))
                    {
                        _Value = value;
                        OnValueChanged();
                    }
                }
            }

            public ApiCacheState _State;
            public ApiCacheState State
            {
                get
                {
                    return _State;
                }
                set
                {
                    if (_State != value)
                    {
                        _State = value;
                        OnValueChanged();
                    }
                }
            }

            public ApiValue<T> Clone()
            {
                var a = new ApiValue<T>();

                a._LastChange = _LastChange;
                a._State = _State;
                a._Value = _Value;

                return a;
            }
        }

        private class AccountApiData : IAccountApiData
        {
            public IApiValue<T> CreateValue<T>()
            {
                return new ApiValue<T>();
            }

            public ApiValue<ushort> _DailyPoints;
            public IApiValue<ushort> DailyPoints
            {
                get
                {
                    return _DailyPoints;
                }
                set
                {
                    if (_DailyPoints != value)
                    {
                        _DailyPoints = (ApiValue<ushort>)value;
                        OnValueChanged();
                    }
                }
            }

            public ApiValue<int> _Played;
            public IApiValue<int> Played
            {
                get
                {
                    return _Played;
                }
                set
                {
                    if (_Played != value)
                    {
                        _Played = (ApiValue<int>)value;
                        OnValueChanged();
                    }
                }
            }

            public AccountApiData Clone()
            {
                var a = new AccountApiData();

                if (_DailyPoints != null)
                    a._DailyPoints = _DailyPoints.Clone();
                if (_Played != null)
                    a._Played = _Played.Clone();

                return a;
            }
        }

        public interface IAccount
        {
            event EventHandler NameChanged,
                               IconChanged,
                               IconTypeChanged,
                               ColorKeyChanged,
                               LastUsedUtcChanged,
                               JumpListPinningChanged;

            /// <summary>
            /// Unique identifier
            /// </summary>
            ushort UID
            {
                get;
            }

            /// <summary>
            /// Displayed name
            /// </summary>
            string Name
            {
                get;
                set;
            }

            /// <summary>
            /// Name of a Windows account to use
            /// </summary>
            string WindowsAccount
            {
                get;
                set;
            }

            /// <summary>
            /// The last time (in UTC) this account was used
            /// </summary>
            DateTime LastUsedUtc
            {
                get;
                set;
            }

            /// <summary>
            /// The date (in UTC) this account was created
            /// </summary>
            DateTime CreatedUtc
            {
                get;
                set;
            }

            /// <summary>
            /// Command line arguments
            /// </summary>
            string Arguments
            {
                get;
                set;
            }

            /// <summary>
            /// True if using -windowed mode
            /// </summary>
            bool Windowed
            {
                get;
            }

            /// <summary>
            /// Options for -windowed mode
            /// </summary>
            WindowOptions WindowOptions
            {
                get;
                set;
            }

            /// <summary>
            /// The bounds of the -windowed mode window
            /// </summary>
            Rectangle WindowBounds
            {
                get;
                set;
            }

            /// <summary>
            /// The number of times the account has been launched
            /// </summary>
            ushort TotalUses
            {
                get;
                set;
            }

            /// <summary>
            /// Shows an indicator when the account's last used date is before the daily reset
            /// </summary>
            bool ShowDailyLogin
            {
                get;
                set;
            }

            /// <summary>
            /// Records the launch and exit times
            /// </summary>
            bool RecordLaunches
            {
                get;
                set;
            }

            AutologinOptions AutologinOptions
            {
                get;
                set;
            }

            /// <summary>
            /// Automatically login
            /// </summary>
            bool AutomaticLogin
            {
                get;
                set;
            }

            /// <summary>
            /// True if the email and password isn't empty
            /// </summary>
            bool HasCredentials
            {
                get;
            }

            /// <summary>
            /// The email for the account
            /// </summary>
            string Email
            {
                get;
                set;
            }

            /// <summary>
            /// The password for the account
            /// </summary>
            PasswordString Password
            {
                get;
                set;
            }

            /// <summary>
            /// True to set the volume in Windows
            /// </summary>
            bool VolumeEnabled
            {
                get;
                set;
            }

            /// <summary>
            /// Volumne between 0.0 and 1.0
            /// </summary>
            float Volume
            {
                get;
                set;
            }

            /// <summary>
            /// Programs to run after launching
            /// </summary>
            RunAfter[] RunAfter
            {
                get;
                set;
            }

            /// <summary>
            /// Enables sounds options
            /// </summary>
            MuteOptions Mute
            {
                get;
                set;
            }

            /// <summary>
            /// Enables the option to change the screenshot format
            /// </summary>
            ScreenshotFormat ScreenshotsFormat
            {
                get;
                set;
            }

            /// <summary>
            /// The location where screenshots are saved
            /// </summary>
            string ScreenshotsLocation
            {
                get;
                set;
            }

            /// <summary>
            /// True if any files need to be updated when launching
            /// </summary>
            bool PendingFiles
            {
                get;
                set;
            }

            /// <summary>
            /// The state of the current network (whether it needs to be authenticated)
            /// </summary>
            NetworkAuthorizationState NetworkAuthorizationState
            {
                get;
                set;
            }

            /// <summary>
            /// Authenticator key for the account
            /// </summary>
            byte[] TotpKey
            {
                get;
                set;
            }

            /// <summary>
            /// The priority of the process
            /// </summary>
            ProcessPriorityClass ProcessPriority
            {
                get;
                set;
            }

            /// <summary>
            /// The processor affinity of the process
            /// </summary>
            long ProcessAffinity
            {
                get;
                set;
            }

            /// <summary>
            /// Notes attached to the account
            /// </summary>
            Notes Notes
            {
                get;
                set;
            }

            /// <summary>
            /// The color identifier for the account
            /// </summary>
            Color ColorKey
            {
                get;
                set;
            }

            /// <summary>
            /// The type of icon for the account
            /// </summary>
            IconType IconType
            {
                get;
                set;
            }

            /// <summary>
            /// The icon for the account
            /// </summary>
            string Icon
            {
                get;
                set;
            }

            /// <summary>
            /// The button image for the account
            /// </summary>
            ImageOptions Image
            {
                get;
                set;
            }

            /// <summary>
            /// The button background image for the account
            /// </summary>
            string BackgroundImage
            {
                get;
                set;
            }

            /// <summary>
            /// The key used when sorted manually
            /// </summary>
            ushort SortKey
            {
                get;
                set;
            }

            /// <summary>
            /// Adjusts the sort key for the account based on the referenced account
            /// </summary>
            /// <param name="reference">The account that will be used as a reference</param>
            /// <param name="type">Where the account should be ordered based on the referenced account</param>
            void Sort(IAccount reference, AccountSorting.SortType type);

            /// <summary>
            /// Manually adjusts the sort key
            /// </summary>
            /// <param name="index">Ordered index when sorted</param>
            void Sort(ushort index);

            /// <summary>
            /// JumpList state
            /// </summary>
            JumpListPinning JumpListPinning
            {
                get;
                set;
            }

            /// <summary>
            /// Type of account
            /// </summary>
            AccountType Type
            {
                get;
            }

            /// <summary>
            /// Pages account is displayed on
            /// </summary>
            PageData[] Pages
            {
                get;
                set;
            }
        }

        public interface IGw2Account : IAccount
        {
            /// <summary>
            /// The name of the owned Local.dat file or null if this account
            /// uses whatever is available
            /// </summary>
            IDatFile DatFile
            {
                get;
                set;
            }

            /// <summary>
            /// The name of the owned GFXSettings.xml file or null if this account
            /// uses whatever is available
            /// </summary>
            IGfxFile GfxFile
            {
                get;
                set;
            }

            /// <summary>
            /// Shows an indicator when the daily hasn't been completed
            /// </summary>
            bool ShowDailyCompletion
            {
                get;
                set;
            }

            DateTime LastDailyCompletionUtc
            {
                get;
                set;
            }

            /// <summary>
            /// API data
            /// </summary>
            IAccountApiData ApiData
            {
                get;
                set;
            }

            /// <summary>
            /// API key for the account
            /// </summary>
            string ApiKey
            {
                get;
                set;
            }

            IAccountApiData CreateApiData();

            /// <summary>
            /// Enables the -autologin option
            /// </summary>
            bool AutomaticRememberedLogin
            {
                get;
                set;
            }

            /// <summary>
            /// Enables the -clientport option with the port
            /// </summary>
            ushort ClientPort
            {
                get;
                set;
            }

            /// <summary>
            /// Automatically starts the game after logging in
            /// </summary>
            bool AutomaticPlay
            {
                get;
                set;
            }

            /// <summary>
            /// Enables the -mumble option
            /// </summary>
            string MumbleLinkName
            {
                get;
                set;
            }
        }

        public interface IGw1Account : IAccount
        {
            /// <summary>
            /// The name of the owned Gw.dat file
            /// </summary>
            IGwDatFile DatFile
            {
                get;
                set;
            }

            /// <summary>
            /// Login character name
            /// </summary>
            string CharacterName
            {
                get;
                set;
            }
        }

        public interface IFile
        {
            ushort UID
            {
                get;
            }

            string Path
            {
                get;
                set;
            }

            /// <summary>
            /// File has been initialized by the game
            /// </summary>
            bool IsInitialized
            {
                get;
                set;
            }

            /// <summary>
            /// File has been updated and requires applicable changes
            /// </summary>
            bool IsPending
            {
                get;
                set;
            }

            byte References
            {
                get;
            }

            /// <summary>
            /// Not implemented or used
            /// </summary>
            bool IsLocked
            {
                get;
                set;
            }
        }

        public interface IDatFile : IFile
        {
        }

        public interface IGfxFile : IFile
        {
        }

        public interface IGwDatFile : IFile
        {
        }

        public interface IKeyedProperty<TKey,TValue>
        {
            event EventHandler<KeyValuePair<TKey, ISettingValue<TValue>>> ValueChanged, ValueAdded;
            event EventHandler<TKey> ValueRemoved;

            bool Contains(TKey key);

            ISettingValue<TValue> this[TKey key]
            {
                get;
            }

            int Count
            {
                get;
            }

            TKey[] GetKeys();
        }

        public interface IHashSetProperty<T>
        {
            event EventHandler<T> ValueAdded, ValueRemoved;

            bool this[T item]
            {
                get;
                set;
            }

            bool Contains(T item);

            bool Add(T item);

            bool Remove(T item);

            int Count
            {
                get;
            }
        }

        public interface IListProperty<T>
        {
            event EventHandler<KeyValuePair<int, T>> ValueChanged;
            event EventHandler<T> ValueAdded, ValueRemoved;

            T this[int index]
            {
                get;
                set;
            }

            bool Contains(T item);

            void Add(T item);

            bool Remove(T item);

            int IndexOf(T item);

            void ReplaceOrAdd(T oldValue, T newValue);

            int Count
            {
                get;
            }
        }

        public interface IPendingSettingValue<T> : ISettingValue<T>
        {
            T ValueCommit
            {
                get;
            }

            bool IsPending
            {
                get;
            }

            /// <summary>
            /// Saves the current pending value
            /// </summary>
            void Commit();

            /// <summary>
            /// Flags the current value as pending
            /// </summary>
            void SetPending();
        }

        public interface ISettingValue<T>
        {
            event EventHandler ValueChanged;
            event EventHandler<T> ValueCleared;

            T Value
            {
                get;
                set;
            }

            bool HasValue
            {
                get;
            }

            void Clear();
        }

        public interface IKeyedSettingValue<TKey, TValue> : ISettingValue<TValue>
        {
            TKey Key
            {
                get;
            }
        }

        private class KeyedProperty<TKey, TValue> : IKeyedProperty<TKey, TValue>, IEnumerable<KeyValuePair<TKey, ISettingValue<TValue>>>
        {
            public event EventHandler<KeyValuePair<TKey, ISettingValue<TValue>>> ValueChanged, ValueAdded;
            public event EventHandler<TKey> ValueRemoved;

            private Dictionary<TKey, ISettingValue<TValue>> _dictionary;
            private Func<TKey, ISettingValue<TValue>> onNewKey;

            public KeyedProperty()
            {
                _dictionary = new Dictionary<TKey, ISettingValue<TValue>>();
            }

            public KeyedProperty(IEqualityComparer<TKey> comparer)
            {
                _dictionary = new Dictionary<TKey, ISettingValue<TValue>>(comparer);
            }

            public KeyedProperty(IEqualityComparer<TKey> comparer, Func<TKey, ISettingValue<TValue>> onNewKey)
                : this(comparer)
            {
                this.onNewKey = onNewKey;
            }

            public KeyedProperty(Func<TKey, ISettingValue<TValue>> onNewKey)
                : this()
            {
                this.onNewKey = onNewKey;
            }

            public virtual ISettingValue<TValue> OnCreateNewValue(TKey key)
            {
                if (onNewKey != null)
                    return onNewKey(key);
                return new SettingValue<TValue>();
            }

            public void Add(TKey key, ISettingValue<TValue> value)
            {
                lock (this)
                {
                    _dictionary.Add(key, value);
                }

                if (ValueAdded != null)
                    ValueAdded(this, new KeyValuePair<TKey, ISettingValue<TValue>>(key, value));
            }

            public bool Remove(TKey key)
            {
                bool removed;

                lock (this)
                {
                    removed = _dictionary.Remove(key);
                }

                if (removed)
                {
                    if (ValueRemoved != null)
                        ValueRemoved(this, key);

                    return true;
                }

                return false;
            }

            public bool Contains(TKey key)
            {
                lock (this)
                {
                    return _dictionary.ContainsKey(key);
                }
            }

            public bool TryGetValue(TKey key, out ISettingValue<TValue> value)
            {
                lock (this)
                {
                    return _dictionary.TryGetValue(key, out value);
                }
            }

            public int Count
            {
                get
                {
                    lock (this)
                    {
                        return _dictionary.Count;
                    }
                }
            }

            public ISettingValue<TValue> this[TKey key]
            {
                get
                {
                    bool changed = false;
                    ISettingValue<TValue> v;
                    lock (this)
                    {
                        if (!_dictionary.TryGetValue(key, out v))
                        {
                            v = OnCreateNewValue(key);
                            _dictionary.Add(key, v);
                            changed = true;
                        }
                    }

                    if (changed && ValueChanged != null)
                        ValueChanged(this, new KeyValuePair<TKey, ISettingValue<TValue>>(key, v));

                    return v;
                }
                set
                {
                    bool changed = false;

                    lock (this)
                    {
                        ISettingValue<TValue> v;
                        if (_dictionary.TryGetValue(key, out v))
                        {
                            if (!v.Equals(value))
                            {
                                _dictionary[key] = value;
                                changed = true;
                            }
                        }
                        else
                        {
                            _dictionary.Add(key, value);
                            changed = true;
                        }
                    }

                    if (changed)
                    {
                        OnValueChanged();
                        if (ValueChanged != null)
                            ValueChanged(this, new KeyValuePair<TKey, ISettingValue<TValue>>(key, value));
                    }
                }
            }

            public Dictionary<TKey, ISettingValue<TValue>>.KeyCollection Keys
            {
                get
                {
                    return _dictionary.Keys;
                }
            }

            IEnumerator<KeyValuePair<TKey, ISettingValue<TValue>>> IEnumerable<KeyValuePair<TKey, ISettingValue<TValue>>>.GetEnumerator()
            {
                return _dictionary.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return _dictionary.GetEnumerator();
            }

            public TKey[] GetKeys()
            {
                return _dictionary.Keys.ToArray<TKey>();
            }

            public void Clear()
            {
                _dictionary.Clear();
            }
        }

        private class HashSetProperty<T> : IHashSetProperty<T>, IEnumerable<T>
        {
            public event EventHandler<T> ValueAdded, ValueRemoved;

            private HashSet<T> _hashset;

            public HashSetProperty()
            {
                _hashset = new HashSet<T>();
            }

            public bool Add(T item)
            {
                bool b;

                lock (this)
                {
                     b = _hashset.Add(item);
                }

                if (b)
                {
                    if (ValueAdded != null)
                        ValueAdded(this, item);
                    OnValueChanged();
                }

                return b;
            }

            public bool Remove(T item)
            {
                bool b;

                lock (this)
                {
                    b = _hashset.Remove(item);
                }

                if (b)
                {
                    if (ValueRemoved != null)
                        ValueRemoved(this, item);
                    OnValueChanged();
                }

                return b;
            }

            public bool Contains(T item)
            {
                lock (this)
                {
                    return _hashset.Contains(item);
                }
            }

            public int Count
            {
                get
                {
                    lock (this)
                    {
                        return _hashset.Count;
                    }
                }
            }

            public bool this[T item]
            {
                get
                {
                    return Contains(item);
                }
                set
                {
                    if (value)
                    {
                        Add(item);
                    }
                    else
                    {
                        Remove(item);
                    }
                }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return _hashset.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return _hashset.GetEnumerator();
            }
        }

        private class ListProperty<T> : IListProperty<T>, IEnumerable<T>
        {
            public event EventHandler<KeyValuePair<int, T>> ValueChanged;
            public event EventHandler<T> ValueAdded, ValueRemoved;

            public List<T> _list;

            public ListProperty()
            {
                _list = new List<T>();
            }

            public void Add(T item)
            {
                lock (this)
                {
                    _list.Add(item);
                }

                if (ValueAdded != null)
                    ValueAdded(this, item);
                OnValueChanged();
            }

            public bool Remove(T item)
            {
                bool b;

                lock (this)
                {
                    b = _list.Remove(item);
                }

                if (b)
                {
                    if (ValueRemoved != null)
                        ValueRemoved(this, item);
                    OnValueChanged();
                }

                return b;
            }

            public void Clear()
            {
                lock (this)
                {
                    _list.Clear();
                }
            }

            public void ReplaceOrAdd(T oldValue, T newValue)
            {
                int i;

                lock (this)
                {
                    i = _list.IndexOf(oldValue);
                    
                    if (i != -1)
                    {
                        _list[i] = newValue;
                    }
                }

                if (i == -1)
                {
                    Add(newValue);
                }
                else
                {
                    if (ValueChanged != null)
                        ValueChanged(this, new KeyValuePair<int, T>(i, newValue));
                    OnValueChanged();
                }
            }

            public int IndexOf(T item)
            {
                lock (this)
                {
                    return _list.IndexOf(item);
                }
            }

            public bool Contains(T item)
            {
                lock (this)
                {
                    return _list.Contains(item);
                }
            }

            public int Count
            {
                get
                {
                    lock (this)
                    {
                        return _list.Count;
                    }
                }
            }

            public T this[int index]
            {
                get
                {
                    lock (this)
                    {
                        return _list[index];
                    }
                }
                set
                {
                    lock (this)
                    {
                        _list[index] = value;
                    }
                    if (ValueChanged != null)
                        ValueChanged(this, new KeyValuePair<int, T>(index, value));
                    OnValueChanged();
                }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return _list.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return _list.GetEnumerator();
            }
        }

        private class SettingValue<T> : ISettingValue<T>
        {
            public event EventHandler ValueChanged;
            public event EventHandler<T> ValueCleared;

            protected T value;
            protected bool hasValue;

            public SettingValue()
            {

            }

            public SettingValue(T value)
            {
                this.value = value;
                this.hasValue = true;
            }

            protected virtual void OnValueChanged()
            {
                if (ValueChanged != null)
                    ValueChanged(this, EventArgs.Empty);
                Gw2Launcher.Settings.OnValueChanged();
            }

            public virtual T Value
            {
                get
                {
                    return this.value;
                }
                set
                {
                    if (!this.hasValue || !this.value.Equals(value))
                    {
                        this.hasValue = true;
                        this.value = value;

                        OnValueChanged();
                    }
                }
            }

            public virtual void Clear()
            {
                if (this.hasValue)
                {
                    var v = this.value;

                    this.hasValue = false;
                    this.value = default(T);

                    OnValueChanged();

                    if (ValueCleared != null)
                        ValueCleared(this, v);
                }
            }

            public virtual bool HasValue
            {
                get
                {
                    return hasValue;
                }
            }

            public virtual void SetValue(T value)
            {
                this.value = value;
                this.hasValue = true;
            }
        }

        private class PendingSettingValue<T> : SettingValue<T>, IPendingSettingValue<T>
        {
            protected T valueCommit;
            protected bool isPending;

            public PendingSettingValue()
                : base()
            {
            }

            public PendingSettingValue(T value)
                : base(value)
            {
            }

            public override void Clear()
            {
                if (hasValue && !isPending)
                {
                    isPending = true;
                    valueCommit = value;
                }

                base.Clear();
            }

            public override T Value
            {
                get
                {
                    return base.value;
                }
                set
                {
                    if (!base.hasValue || !object.Equals(base.value, value))
                    {
                        if (this.isPending)
                        {
                            if (object.Equals(value, this.valueCommit))
                            {
                                this.valueCommit = default(T);
                                this.isPending = false;
                            }
                        }
                        else
                        {
                            this.valueCommit = base.value;
                            this.isPending = true;
                        }

                        base.hasValue = true;
                        base.value = value;

                        OnValueChanged();
                    }
                }
            }

            public T ValueCommit
            {
                get
                {
                    if (isPending)
                        return this.valueCommit;
                    else
                        return base.value;
                }
            }

            public void SetCommit(T value)
            {
                isPending = true;
                valueCommit = value;
            }

            public void ClearCommit()
            {
                if (isPending)
                {
                    isPending = false;
                    valueCommit = default(T);
                }
            }

            public bool IsPending
            {
                get
                {
                    return isPending;
                }
            }

            public void Commit()
            {
                if (isPending)
                {
                    valueCommit = default(T);
                    isPending = false;

                    Gw2Launcher.Settings.OnValueChanged();
                }
            }

            public void SetPending()
            {
                if (!isPending)
                {
                    isPending = true;
                    valueCommit = value;
                }
            }
        }

        private class KeyedSettingValue<TKey, TValue> : SettingValue<TValue>, IKeyedSettingValue<TKey, TValue>
        {
            public KeyedSettingValue(TKey key)
            {
                this.Key = key;
            }

            public TKey Key
            {
                get;
                protected set;
            }

            public byte ReferenceCount
            {
                get;
                private set;
            }

            public void AddReference(object o)
            {
                lock (this)
                {
                    ReferenceCount++;
                }
            }

            public void RemoveReference(object o)
            {
                lock (this)
                {
                    ReferenceCount--;
                }
            }
        }

        public static class AccountSorting
        {
            public static event EventHandler SortingChanged;

            public enum SortType
            {
                Before,
                After,
                Swap,
            }

            /// <summary>
            /// Notifies that sorting has been modified
            /// </summary>
            public static void Update()
            {
                OnValueChanged();

                if (SortingChanged != null)
                    SortingChanged(null, EventArgs.Empty);
            }

            public static void Sort(IAccount account, ushort index)
            {
                var _account = (Account)account;
                if (_account._SortKey == index)
                    return;

                _account._SortKey = index;

                OnValueChanged();

                if (SortingChanged != null)
                    SortingChanged(null, EventArgs.Empty);
            }

            public static void Sort(IAccount account, IAccount reference, SortType type)
            {
                var _reference = (Account)reference;
                var _account = (Account)account;
                var index = _reference._SortKey;

                switch (type)
                {
                    case AccountSorting.SortType.After:

                        if (_account._SortKey > index)
                            ++index;

                        break;
                    case AccountSorting.SortType.Before:

                        if (_account._SortKey < index)
                            --index;

                        break;
                    case AccountSorting.SortType.Swap:

                        _reference._SortKey = _account._SortKey;
                        _account._SortKey = index;

                        OnValueChanged();

                        if (SortingChanged != null)
                            SortingChanged(null, EventArgs.Empty);

                        return;
                    default:
                        throw new NotSupportedException();
                }

                if (_account._SortKey == index)
                    return;

                if (_account._SortKey > index)
                {
                    foreach (var key in _Accounts.Keys)
                    {
                        var a = (Account)_Accounts[key].Value;
                        if (a != null)
                        {
                            if (a._SortKey >= index && a._SortKey < _account._SortKey)
                                ++a._SortKey;
                        }
                    }
                }
                else
                {
                    foreach (var key in _Accounts.Keys)
                    {
                        var a = (Account)_Accounts[key].Value;
                        if (a != null)
                        {
                            if (a._SortKey <= index && a._SortKey > _account._SortKey)
                                --a._SortKey;
                        }
                    }
                }

                _account._SortKey = index;

                var total = _Accounts.Count;
                total = total * (total + 1) / 2;

                foreach (var key in _Accounts.Keys)
                {
                    var a = (Account)_Accounts[key].Value;
                    if (a != null)
                    {
                        total -= a._SortKey;
                    }
                }

                OnValueChanged();

                if (SortingChanged != null)
                    SortingChanged(null, EventArgs.Empty);
            }
        }

        private class Account : IAccount
        {
            public event EventHandler NameChanged,
                                      IconChanged,
                                      IconTypeChanged,
                                      ColorKeyChanged,
                                      LastUsedUtcChanged,
                                      JumpListPinningChanged;

            public Account(AccountType type, ushort uid)
                : this(type)
            {
                this._UID = uid;
            }

            public Account(AccountType type)
            {
                this._Type = type;
            }

            public ushort _UID;
            public ushort UID
            {
                get
                {
                    return _UID;
                }
                set
                {
                    if (_UID != value)
                    {
                        _UID = value;
                        OnValueChanged();
                    }
                }
            }

            public string _Name;
            public string Name
            {
                get
                {
                    return _Name;
                }
                set
                {
                    if (_Name != value)
                    {
                        _Name = value;
                        OnValueChanged();

                        if (NameChanged != null)
                            NameChanged(this, EventArgs.Empty);
                    }
                }
            }

            public string _WindowsAccount;
            public string WindowsAccount
            {
                get
                {
                    return _WindowsAccount;
                }
                set
                {
                    if (_WindowsAccount != value)
                    {
                        _WindowsAccount = value;
                        _PendingFiles = true;
                        OnValueChanged();
                    }
                }
            }

            public DateTime _LastUsedUtc;
            public DateTime LastUsedUtc
            {
                get
                {
                    return _LastUsedUtc;
                }
                set
                {
                    if (_LastUsedUtc != value)
                    {
                        _LastUsedUtc = value;
                        OnValueChanged();

                        if (LastUsedUtcChanged != null)
                            LastUsedUtcChanged(this, EventArgs.Empty);
                    }
                }
            }

            public string _Arguments;
            public string Arguments
            {
                get
                {
                    return _Arguments;
                }
                set
                {
                    if (_Arguments != value)
                    {
                        _Arguments = value;
                        OnValueChanged();
                    }
                }
            }

            public bool Windowed
            {
                get
                {
                    return _WindowOptions.HasFlag(Gw2Launcher.Settings.WindowOptions.Windowed);
                }
                set
                {
                    if (Windowed != value)
                    {
                        if (value)
                            _WindowOptions |= Gw2Launcher.Settings.WindowOptions.Windowed;
                        else
                            _WindowOptions &= ~Gw2Launcher.Settings.WindowOptions.Windowed;

                        OnValueChanged();
                    }
                }
            }

            public WindowOptions _WindowOptions;
            public WindowOptions WindowOptions
            {
                get
                {
                    return _WindowOptions;
                }
                set
                {
                    if (_WindowOptions != value)
                    {
                        _WindowOptions = value;
                        OnValueChanged();
                    }
                }
            }

            public Rectangle _WindowBounds;
            public Rectangle WindowBounds
            {
                get
                {
                    return _WindowBounds;
                }
                set
                {
                    if (_WindowBounds != value)
                    {
                        _WindowBounds = value;
                        OnValueChanged();
                    }
                }
            }

            public ushort _TotalUses;
            public ushort TotalUses
            {
                get
                {
                    return _TotalUses;
                }
                set
                {
                    if (_TotalUses != value)
                    {
                        _TotalUses = value;
                        OnValueChanged();
                    }
                }
            }

            public bool _ShowDailyLogin;
            public bool ShowDailyLogin
            {
                get
                {
                    return _ShowDailyLogin;
                }
                set
                {
                    if (_ShowDailyLogin != value)
                    {
                        _ShowDailyLogin = value;
                        OnValueChanged();
                    }
                }
            }

            public bool _RecordLaunches;
            public bool RecordLaunches
            {
                get
                {
                    return _RecordLaunches;
                }
                set
                {
                    if (_RecordLaunches != value)
                    {
                        _RecordLaunches = value;
                        OnValueChanged();
                    }
                }
            }

            public AutologinOptions _AutologinOptions;
            public AutologinOptions AutologinOptions
            {
                get
                {
                    return _AutologinOptions;
                }
                set
                {
                    if (_AutologinOptions != value)
                    {
                        _AutologinOptions = value;
                        OnValueChanged();
                    }
                }
            }

            public bool AutomaticLogin
            {
                get
                {
                    return _AutologinOptions.HasFlag(Gw2Launcher.Settings.AutologinOptions.Login);
                }
                set
                {
                    if (value)
                    {
                        AutologinOptions |= Gw2Launcher.Settings.AutologinOptions.Login;
                    }
                    else
                    {
                        AutologinOptions &= ~Gw2Launcher.Settings.AutologinOptions.Login;
                    }
                }
            }

            public bool HasCredentials
            {
                get
                {
                    return !string.IsNullOrEmpty(_Email) && _Password != null && !_Password.Data.IsEmpty;
                }
            }

            public string _Email;
            public string Email
            {
                get
                {
                    return _Email;
                }
                set
                {
                    if (_Email != value)
                    {
                        _Email = value;
                        OnValueChanged();
                    }
                }
            }

            public PasswordString _Password;
            public PasswordString Password
            {
                get
                {
                    return _Password;
                }
                set
                {
                    if (_Password != value)
                    {
                        _Password = value;
                        OnValueChanged();
                    }
                }
            }

            public DateTime _CreatedUtc;
            public DateTime CreatedUtc
            {
                get
                {
                    return _CreatedUtc;
                }
                set
                {
                    if (_CreatedUtc != value)
                    {
                        _CreatedUtc = value;
                        OnValueChanged();
                    }
                }
            }

            public bool VolumeEnabled
            {
                get
                {
                    return _Volume != 0;
                }
                set
                {
                    if (!value)
                        _Volume = 0;
                    else if (_Volume == 0)
                        _Volume = 1;
                }
            }

            //volume is stored with a +1 offset, where 0 is disabled
            public byte _Volume;
            public float Volume
            {
                get
                {
                    if (_Volume > 0)
                        return (_Volume - 1) / 254f;
                    else
                        return _Volume;
                }
                set
                {
                    byte v;
                    if (value >= 1)
                        v = 255;
                    else
                        v = (byte)(value * 254 + 1);
                    if (_Volume != v)
                    {
                        _Volume = v;
                        OnValueChanged();
                    }
                }
            }

            public RunAfter[] _RunAfter;
            public RunAfter[] RunAfter
            {
                get
                {
                    return _RunAfter;
                }
                set
                {
                    if (_RunAfter != value)
                    {
                        _RunAfter = value;
                        OnValueChanged();
                    }
                }
            }

            public MuteOptions _Mute;
            public MuteOptions Mute
            {
                get
                {
                    return _Mute;
                }
                set
                {
                    if (_Mute != value)
                    {
                        _Mute = value;
                        OnValueChanged();
                    }
                }
            }

            public ScreenshotFormat _ScreenshotsFormat;
            public ScreenshotFormat ScreenshotsFormat
            {
                get
                {
                    return _ScreenshotsFormat;
                }
                set
                {
                    if (_ScreenshotsFormat != value)
                    {
                        _ScreenshotsFormat = value;
                        OnValueChanged();
                    }
                }
            }

            public string _ScreenshotsLocation;
            public string ScreenshotsLocation
            {
                get
                {
                    return _ScreenshotsLocation;
                }
                set
                {
                    if (_ScreenshotsLocation != value)
                    {
                        _ScreenshotsLocation = value;
                        _PendingFiles = true;
                        OnValueChanged();
                    }
                }
            }

            public bool _PendingFiles;
            public bool PendingFiles
            {
                get
                {
                    return _PendingFiles;
                }
                set
                {
                    if (_PendingFiles != value)
                    {
                        _PendingFiles = value;
                        OnValueChanged();
                    }
                }
            }

            public NetworkAuthorizationState _NetworkAuthorizationState;
            public NetworkAuthorizationState NetworkAuthorizationState
            {
                get
                {
                    return _NetworkAuthorizationState;
                }
                set
                {
                    if (_NetworkAuthorizationState != value)
                    {
                        _NetworkAuthorizationState = value;
                        OnValueChanged();
                    }
                }
            }

            public byte[] _TotpKey;
            public byte[] TotpKey
            {
                get
                {
                    return _TotpKey;
                }
                set
                {
                    if (_TotpKey != value)
                    {
                        _TotpKey = value;
                        OnValueChanged();
                    }
                }
            }

            public ProcessPriorityClass _ProcessPriority;
            public ProcessPriorityClass ProcessPriority
            {
                get
                {
                    return _ProcessPriority;
                }
                set
                {
                    if (_ProcessPriority != value)
                    {
                        _ProcessPriority = value;
                        OnValueChanged();
                    }
                }
            }

            public long _ProcessAffinity;
            public long ProcessAffinity
            {
                get
                {
                    return _ProcessAffinity;
                }
                set
                {
                    if (_ProcessAffinity != value)
                    {
                        _ProcessAffinity = value;
                        OnValueChanged();
                    }
                }
            }

            public Notes _Notes;
            public Notes Notes
            {
                get
                {
                    return _Notes;
                }
                set
                {
                    if (_Notes != value)
                    {
                        _Notes = value;
                        OnValueChanged();
                    }
                }
            }

            public Color _ColorKey;
            public Color ColorKey
            {
                get
                {
                    return _ColorKey;
                }
                set
                {
                    if (_ColorKey != value)
                    {
                        _ColorKey = value;
                        OnValueChanged();

                        if (ColorKeyChanged != null)
                            ColorKeyChanged(this, EventArgs.Empty);
                    }
                }
            }

            public IconType _IconType;
            public IconType IconType
            {
                get
                {
                    return _IconType;
                }
                set
                {
                    if (_IconType != value)
                    {
                        _IconType = value;
                        OnValueChanged();

                        if (IconTypeChanged != null)
                            IconTypeChanged(this, EventArgs.Empty);
                    }
                }
            }

            public string _Icon;
            public string Icon
            {
                get
                {
                    return _Icon;
                }
                set
                {
                    if (_Icon != value)
                    {
                        _Icon = value;
                        OnValueChanged();

                        if (IconChanged != null)
                            IconChanged(this, EventArgs.Empty);
                    }
                }
            }

            public ushort _SortKey;
            public ushort SortKey
            {
                get
                {
                    return _SortKey;
                }
                set
                {
                    if (_SortKey != value)
                    {
                        _SortKey = value;
                        OnValueChanged();
                    }
                }
            }

            public void Sort(IAccount reference, AccountSorting.SortType type)
            {
                AccountSorting.Sort(this, reference, type);
            }

            public void Sort(ushort index)
            {
                AccountSorting.Sort(this, index);
            }

            public JumpListPinning _JumpListPinning;
            public JumpListPinning JumpListPinning
            {
                get
                {
                    return _JumpListPinning;
                }
                set
                {
                    if (_JumpListPinning != value)
                    {
                        _JumpListPinning = value;
                        OnValueChanged();

                        if (JumpListPinningChanged != null)
                            JumpListPinningChanged(this, EventArgs.Empty);
                    }
                }
            }

            public AccountType _Type;
            public AccountType Type
            {
                get
                {
                    return _Type;
                }
                set
                {
                    if (_Type != value)
                    {
                        _Type = value;
                        OnValueChanged();
                    }
                }
            }

            public ImageOptions _Image;
            public ImageOptions Image
            {
                get
                {
                    return _Image;
                }
                set
                {
                    if (_Image != value)
                    {
                        _Image = value;
                        OnValueChanged();
                    }
                }
            }

            public string _BackgroundImage;
            public string BackgroundImage
            {
                get
                {
                    return _BackgroundImage;
                }
                set
                {
                    if (_BackgroundImage != value)
                    {
                        _BackgroundImage = value;
                        OnValueChanged();
                    }
                }
            }

            public PageData[] _Pages;
            public PageData[] Pages
            {
                get
                {
                    return _Pages;
                }
                set
                {
                    _Pages = value;
                    OnValueChanged();
                }
            }

            protected void CloneTo(Account a)
            {
                a._Arguments = _Arguments;
                a._AutologinOptions = _AutologinOptions;
                a._ColorKey = _ColorKey;
                a._CreatedUtc = _CreatedUtc;
                a._Email = _Email;
                a._Icon = _Icon;
                a._IconType = _IconType;
                a._LastUsedUtc = _LastUsedUtc;
                a._Mute = _Mute;
                a._Name = _Name;
                a._NetworkAuthorizationState = _NetworkAuthorizationState;
                a._Password = _Password;
                a._ProcessAffinity = _ProcessAffinity;
                a._ProcessPriority = _ProcessPriority;
                a._RecordLaunches = _RecordLaunches;
                a._RunAfter = _RunAfter; //read-only
                a._ScreenshotsFormat = _ScreenshotsFormat;
                a._ScreenshotsLocation = _ScreenshotsLocation;
                a._ShowDailyLogin = _ShowDailyLogin;
                a._SortKey = _SortKey;
                a._TotalUses = _TotalUses;
                a._TotpKey = _TotpKey;
                a._UID = _UID;
                a._Volume = _Volume;
                a._WindowBounds = _WindowBounds;
                a._WindowOptions = _WindowOptions;
                a._WindowsAccount = _WindowsAccount;
                a._JumpListPinning = _JumpListPinning;
                //_Type is not included
                a._Image = _Image;
                a._BackgroundImage = _BackgroundImage;
                //_Pages is not included

                a._PendingFiles = _PendingFiles;

                if (_Notes != null)
                    a._Notes = _Notes.Clone();
            }

            public virtual Account Clone()
            {
                var a = new Account(_Type);

                CloneTo(a);

                return a;
            }
        }

        private class Gw2Account : Account, IGw2Account
        {
            public Gw2Account()
                : this(0)
            {
            }

            public Gw2Account(ushort uid)
                : base(AccountType.GuildWars2, uid)
            {
                _LastDailyCompletionUtc = new DateTime(1);
            }

            public override Account Clone()
            {
                var a = new Gw2Account();

                CloneTo(a);

                if (_ApiData != null)
                    a._ApiData = _ApiData.Clone();
                a._ApiKey = _ApiKey;
                a._AutomaticRememberedLogin = _AutomaticRememberedLogin;
                a._ClientPort = _ClientPort;
                a._LastDailyCompletionUtc = _LastDailyCompletionUtc;
                a._MumbleLinkName = _MumbleLinkName;

                a.GfxFile = _GfxFile;
                a.DatFile = _DatFile;

                return a;
            }

            public DatFile _DatFile;
            public IDatFile DatFile
            {
                get
                {
                    return _DatFile;
                }
                set
                {
                    if (_DatFile != value)
                    {
                        if (_DatFile != null)
                        {
                            lock (_DatFile)
                            {
                                _DatFile.ReferenceCount--;
                            }
                            _DatFile.PathChanged -= File_PathChanged;
                        }

                        var file = (DatFile)value;
                        if (file != null)
                        {
                            lock (file)
                            {
                                file.ReferenceCount++;
                            }
                            file.PathChanged += File_PathChanged;
                        }

                        _DatFile = file;
                        _PendingFiles = true;
                        OnValueChanged();
                    }
                }
            }

            public GfxFile _GfxFile;
            public IGfxFile GfxFile
            {
                get
                {
                    return _GfxFile;
                }
                set
                {
                    if (_GfxFile != value)
                    {
                        if (_GfxFile != null)
                        {
                            lock (_GfxFile)
                            {
                                _GfxFile.ReferenceCount--;
                            }
                            _GfxFile.PathChanged -= File_PathChanged;
                        }

                        var file = (GfxFile)value;
                        if (file != null)
                        {
                            lock (file)
                            {
                                file.ReferenceCount++;
                            }
                            file.PathChanged += File_PathChanged;
                        }

                        _GfxFile = file;
                        _PendingFiles = true;
                        OnValueChanged();
                    }
                }
            }

            void File_PathChanged(object sender, EventArgs e)
            {
                if (!_PendingFiles)
                {
                    _PendingFiles = true;
                    OnValueChanged();
                }
            }

            public bool ShowDailyCompletion
            {
                get
                {
                    return _LastDailyCompletionUtc.Ticks != 1;
                }
                set
                {
                    if (ShowDailyCompletion != value)
                    {
                        if (value)
                            _LastDailyCompletionUtc = DateTime.MinValue;
                        else
                            _LastDailyCompletionUtc = new DateTime(1);
                        OnValueChanged();
                    }
                }
            }

            public DateTime _LastDailyCompletionUtc;
            public DateTime LastDailyCompletionUtc
            {
                get
                {
                    return _LastDailyCompletionUtc;
                }
                set
                {
                    if (_LastDailyCompletionUtc != value)
                    {
                        _LastDailyCompletionUtc = value;
                        OnValueChanged();
                    }
                }
            }

            public bool AutomaticPlay
            {
                get
                {
                    return _AutologinOptions.HasFlag(Gw2Launcher.Settings.AutologinOptions.Play);
                }
                set
                {
                    if (value)
                        AutologinOptions |= Gw2Launcher.Settings.AutologinOptions.Play;
                    else
                        AutologinOptions &= ~Gw2Launcher.Settings.AutologinOptions.Play;
                }
            }

            public bool _AutomaticRememberedLogin;
            public bool AutomaticRememberedLogin
            {
                get
                {
                    return _AutomaticRememberedLogin;
                }
                set
                {
                    if (_AutomaticRememberedLogin != value)
                    {
                        _AutomaticRememberedLogin = value;
                        OnValueChanged();
                    }
                }
            }

            public ushort _ClientPort;
            public ushort ClientPort
            {
                get
                {
                    return _ClientPort;
                }
                set
                {
                    if (_ClientPort != value)
                    {
                        _ClientPort = value;
                        OnValueChanged();
                    }
                }
            }

            public AccountApiData _ApiData;
            public IAccountApiData ApiData
            {
                get
                {
                    return _ApiData;
                }
                set
                {
                    if (_ApiData != value)
                    {
                        _ApiData = (AccountApiData)value;
                        OnValueChanged();
                    }
                }
            }

            public string _ApiKey;
            public string ApiKey
            {
                get
                {
                    return _ApiKey;
                }
                set
                {
                    if (_ApiKey != value)
                    {
                        _ApiKey = value;
                        OnValueChanged();
                    }
                }
            }

            public IAccountApiData CreateApiData()
            {
                return new AccountApiData();
            }

            public string _MumbleLinkName;
            public string MumbleLinkName
            {
                get
                {
                    return _MumbleLinkName;
                }
                set
                {
                    if (_MumbleLinkName != value)
                    {
                        _MumbleLinkName = value;
                        OnValueChanged();
                    }
                }
            }
        }

        private class Gw1Account : Account, IGw1Account
        {
            public Gw1Account()
                : this(0)
            {
            }

            public Gw1Account(ushort uid)
                : base(AccountType.GuildWars1, uid)
            {
            }

            public override Account Clone()
            {
                var a = new Gw1Account();

                CloneTo(a);

                a.DatFile = _DatFile;
                a.CharacterName = _CharacterName;

                return a;
            }

            public GwDatFile _DatFile;
            public IGwDatFile DatFile
            {
                get
                {
                    return _DatFile;
                }
                set
                {
                    if (_DatFile != value)
                    {
                        if (_DatFile != null)
                        {
                            lock (_DatFile)
                            {
                                _DatFile.ReferenceCount--;
                            }
                            _DatFile.PathChanged -= File_PathChanged;
                        }

                        var file = (GwDatFile)value;
                        if (file != null)
                        {
                            lock (file)
                            {
                                file.ReferenceCount++;
                            }
                            file.PathChanged += File_PathChanged;
                        }

                        _DatFile = file;
                        _PendingFiles = true;
                        OnValueChanged();
                    }
                }
            }

            void File_PathChanged(object sender, EventArgs e)
            {
                if (!_PendingFiles)
                {
                    _PendingFiles = true;
                    OnValueChanged();
                }
            }

            public string _CharacterName;
            public string CharacterName
            {
                get
                {
                    return _CharacterName;
                }
                set
                {
                    if (_CharacterName != value)
                    {
                        _CharacterName = value;
                        OnValueChanged();
                    }
                }
            }
        }

        private class DatFile : BaseFile, IDatFile
        {
            public DatFile(ushort uid)
                : base(uid)
            {
            }

            public DatFile()
            {

            }
        }

        private class GfxFile : BaseFile, IGfxFile
        {
            public GfxFile(ushort uid)
                : base(uid)
            {
            }

            public GfxFile()
            {

            }
        }

        private class GwDatFile : BaseFile, IGwDatFile
        {
            public GwDatFile(ushort uid)
                : base(uid)
            {
            }

            public GwDatFile()
            {

            }
        }

        private abstract class BaseFile : IFile
        {
            public event EventHandler PathChanged;

            [Flags]
            public enum DataFlags : byte
            {
                None = 0,
                Initialized = 1,
                Pending = 2,
                Locked = 4,
            }

            public BaseFile(ushort uid)
            {
                this.UID = uid;
            }

            public BaseFile()
            {

            }

            public byte ReferenceCount;
            public DataFlags _flags;

            public ushort _UID;
            public ushort UID
            {
                get
                {
                    return _UID;
                }
                set
                {
                    if (_UID != value)
                    {
                        _UID = value;
                        OnValueChanged();
                    }
                }
            }

            public string _Path;
            public string Path
            {
                get
                {
                    return _Path;
                }
                set
                {
                    if (_Path != value)
                    {
                        _Path = value;
                        OnValueChanged();

                        if (PathChanged != null)
                            PathChanged(this, EventArgs.Empty);
                    }
                }
            }

            public bool IsInitialized
            {
                get
                {
                    return (_flags & DataFlags.Initialized) == DataFlags.Initialized;
                }
                set
                {
                    lock (this)
                    {
                        if (IsInitialized != value)
                        {
                            if (value)
                            {
                                _flags |= DataFlags.Initialized;
                            }
                            else
                            {
                                _flags = (_flags & ~DataFlags.Initialized) | DataFlags.Pending;
                            }
                            OnValueChanged();
                        }
                    }
                }
            }

            public bool IsPending
            {
                get
                {
                    return (_flags & DataFlags.Pending) == DataFlags.Pending;
                }
                set
                {
                    lock (this)
                    {
                        if (IsPending != value)
                        {
                            if (value)
                                _flags |= DataFlags.Pending;
                            else
                                _flags &= ~DataFlags.Pending;
                            OnValueChanged();
                        }
                    }
                }
            }

            public bool IsLocked
            {
                get
                {
                    return (_flags & DataFlags.Locked) == DataFlags.Locked;
                }
                set
                {
                    lock (this)
                    {
                        if (IsLocked != value)
                        {
                            if (value)
                                _flags |= DataFlags.Locked;
                            else
                                _flags &= ~DataFlags.Locked;
                            OnValueChanged();
                        }
                    }
                }
            }

            public byte References
            {
                get
                {
                    return ReferenceCount;
                }
            }
        }

        public struct Values<T1, T2>
        {
            public T1 value1;
            public T2 value2;
        }

        public class LastCheckedVersion
        {
            public LastCheckedVersion(DateTime lastCheck, ushort version)
            {
                LastCheck = lastCheck;
                Version = version;
            }

            public DateTime LastCheck
            {
                get;
                private set;
            }
            public ushort Version
            {
                get;
                private set;
            }
        }

        public class ScreenAttachment
        {
            public ScreenAttachment(byte screen, ScreenAnchor anchor)
            {
                Screen = screen;
                Anchor = anchor;
            }

            public byte Screen
            {
                get;
                private set;
            }
            public ScreenAnchor Anchor
            {
                get;
                private set;
            }
        }

        public struct Point<T>
        {
            public Point(T x, T y)
                : this()
            {
                X = x;
                Y = y;
            }

            public T X
            {
                get;
                set;
            }

            public T Y
            {
                get;
                set;
            }

            public bool IsEmpty
            {
                get
                {
                    return object.Equals(X, Y) && object.Equals(X, default(T));
                }
            }

            public Point ToPoint()
            {
                return new Point(Convert.ToInt32(X), Convert.ToInt32(Y));
            }
        }

        public class LauncherPoints
        {
            public Point<ushort> EmptyArea
            {
                get;
                set;
            }

            public Point<ushort> PlayButton
            {
                get;
                set;
            }
        }

        public class NotificationScreenAttachment : ScreenAttachment
        {
            public NotificationScreenAttachment(byte screen, ScreenAnchor anchor, bool onlyWhileActive)
                : base(screen, anchor)
            {
                OnlyWhileActive = onlyWhileActive;
            }

            public bool OnlyWhileActive
            {
                get;
                private set;
            }
        }

        public class ScreenshotConversionOptions
        {
            public enum ImageFormat : byte
            {
                None = 0,
                Jpg = 1,
                Png = 2
            }

            public byte raw;

            public ImageFormat Format
            {
                get;
                set;
            }

            /// <summary>
            /// JPG: 0-100 quality, PNG: 24 or 16 color depth
            /// </summary>
            public byte Options
            {
                get
                {
                    return (byte)(raw & 127);
                }
                set
                {
                    if (value > 127)
                        throw new ArgumentOutOfRangeException();
                    raw = (byte)((raw & 128) | value);
                }
            }

            public bool DeleteOriginal
            {
                get
                {
                    return (raw & 128) == 128;
                }
                set
                {
                    if (value)
                        raw |= 128;
                    else
                        raw &= 127;
                }
            }
        }

        private class SettingsFile
        {
            private const string NAME_FORMAT = "{0}.bk";
            private const string FIRST_NAME_FORMAT = "load-{0}.bk";
            private const string PREVIOUS_NAME_FORMAT = "previous-{0}.bk";
            private const string UPGRADE_NAME_FORMAT = "upgrade-{0}.bk";

            private enum BackupType : byte
            {
                None,
                All,

                First,
                Previous,
                Upgrade,
            }

            private class BackupFile
            {
                public BackupFile(string path, long timestamp)
                {
                    this.Path = path;
                    this.Timestamp = timestamp;
                }

                public string Path
                {
                    get;
                    set;
                }

                public long Timestamp
                {
                    get;
                    set;
                }

                public DateTime GetDate()
                {
                    DateTime d;
                    DateTime.TryParseExact(Timestamp.ToString(), "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.AssumeUniversal, out d);
                    return d;
                }
            }

            private BackupType firstSave;
            private DateTime lastBackup;

            public SettingsFile()
            {
            }

            public void Loaded()
            {
                Loaded(0);
            }

            public void Loaded(ushort version)
            {
                if (version > 0)
                {
                    if (version < VERSION)
                    {
                        firstSave = BackupType.Upgrade;
                    }
                    else
                    {
                        var b = GetBackups(BackupType.First, 1);

                        if (b.Length == 0 || DateTime.UtcNow.Subtract(b[0].GetDate()).TotalHours > 6)
                            firstSave = BackupType.First;
                        else
                            firstSave = BackupType.Previous;
                    }
                }
                else
                    firstSave = BackupType.None;

                lastBackup = DateTime.UtcNow;
            }

            public void Save()
            {
                var path = this.Path;
                var tmp = path + ".tmp";

                if (firstSave != BackupType.None)
                {
                    try
                    {
                        Move(path, GetPath(firstSave));
                        Purge(firstSave);

                        lastBackup = DateTime.UtcNow;
                    }
                    catch { }

                    firstSave = BackupType.None;
                }
                else if (DateTime.UtcNow.Subtract(lastBackup).TotalMinutes >= 10)
                {
                    try
                    {
                        Move(path, GetPath(BackupType.Previous));
                        Purge(BackupType.Previous);

                        lastBackup = DateTime.UtcNow;
                    }
                    catch { }
                }

                Move(tmp, path);
            }

            private string GetTimestamp()
            {
                return DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            }

            private string GetFormat(BackupType type)
            {
                string f;

                switch (type)
                {
                    case BackupType.All:
                        f = NAME_FORMAT;
                        break;
                    case BackupType.First:
                        f = FIRST_NAME_FORMAT;
                        break;
                    case BackupType.Upgrade:
                        f = UPGRADE_NAME_FORMAT;
                        break;
                    case BackupType.Previous:
                    default:
                        f = PREVIOUS_NAME_FORMAT;
                        break;
                }

                return f;
            }

            private string GetPath(BackupType type)
            {
                return System.IO.Path.Combine(BackupPath, string.Format(GetFormat(type), GetTimestamp()));
            }

            private void Purge(BackupType type)
            {
                byte limit;

                switch (type)
                {
                    case BackupType.Upgrade:
                        limit = 2;
                        break;
                    default:
                        limit = 3;
                        break;
                }

                var b = GetBackups(type, limit);

                for (var j = limit; j < b.Length; ++j)
                {
                    try
                    {
                        File.Delete(b[j].Path);
                    }
                    catch { }
                }
            }

            private void Move(string from, string to)
            {
                try
                {
                    if (File.Exists(to))
                        File.Delete(to);
                }
                catch { }

                Util.FileUtil.MoveFile(from, to, false, true);
            }

            /// <summary>
            /// Path to settings.dat
            /// </summary>
            public string Path
            {
                get
                {
                    return System.IO.Path.Combine(DataPath.AppData, FILE_NAME);
                }
            }

            /// <summary>
            /// Path to settings.dat.tmp
            /// </summary>
            public string Temp
            {
                get
                {
                    return Path + ".tmp";
                }
            }

            /// <summary>
            /// Path to the backup folder
            /// </summary>
            public string BackupPath
            {
                get
                {
                    var p = System.IO.Path.Combine(DataPath.AppData, "backups");
                    try
                    {
                        Directory.CreateDirectory(p);
                    }
                    catch { }
                    return p;
                }
            }

            private BackupFile[] GetBackups(BackupType type)
            {
                return GetBackups(type, 0);
            }

            /// <summary>
            /// Returns the available backups for the given type in order of newest first
            /// </summary>
            private BackupFile[] GetBackups(BackupType type, int minimum)
            {
                var files = Directory.GetFiles(BackupPath, string.Format(GetFormat(type), "*"));
                if (files.Length < minimum)
                    return new BackupFile[0];
                var backups = new BackupFile[files.Length];
                int count = 0;

                for (var i = files.Length - 1; i >= 0; --i)
                {
                    var n = files[i];
                    var j = n.LastIndexOf('-') + 1;
                    var k = n.LastIndexOf('.');
                    if (j == 0 || k == -1)
                        continue;
                    long l;
                    if (long.TryParse(n.Substring(j, k - j), out l))
                    {
                        backups[count++] = new BackupFile(n, l);
                    }
                }

                if (count != files.Length)
                {
                    var b2 = new BackupFile[count];
                    Array.Copy(backups, 0, b2, 0, count);
                    backups = b2;
                }

                Array.Sort(backups,
                    delegate(BackupFile a, BackupFile b)
                    {
                        return b.Timestamp.CompareTo(a.Timestamp);
                    });

                return backups;
            }

            public IEnumerable<string> GetBackups()
            {
                var backups = GetBackups(BackupType.All, 0);

                foreach (var b in backups)
                {
                    yield return b.Path;
                }
            }
        }

        public interface IAccountTypeSettings
        {
            ISettingValue<string> Path
            {
                get;
            }

            ISettingValue<string> Arguments
            {
                get;
            }

            ISettingValue<RunAfter[]> RunAfter
            {
                get;
            }

            ISettingValue<float> Volume
            {
                get;
            }

            ISettingValue<MuteOptions> Mute
            {
                get;
            }

            ISettingValue<ScreenshotFormat> ScreenshotsFormat
            {
                get;
            }

            ISettingValue<string> ScreenshotsLocation
            {
                get;
            }

            ISettingValue<Settings.ProcessPriorityClass> ProcessPriority
            {
                get;
            }

            ISettingValue<long> ProcessAffinity
            {
                get;
            }
        }

        public interface IGw1Settings : IAccountTypeSettings
        {

        }

        public interface IGw2Settings : IAccountTypeSettings
        {
            ISettingValue<bool> AutomaticRememberedLogin
            {
                get;
            }

            ISettingValue<ushort> ClientPort
            {
                get;
            }

            ISettingValue<bool> PrioritizeCoherentUI
            {
                get;
            }

            ISettingValue<string> MumbleLinkName
            {
                get;
            }

            IPendingSettingValue<string> VirtualUserPath
            {
                get;
            }

            ISettingValue<ProfileMode> ProfileMode
            {
                get;
            }

            ISettingValue<ProfileModeOptions> ProfileOptions
            {
                get;
            }

            IPendingSettingValue<LocalizeAccountExecutionOptions> LocalizeAccountExecution
            {
                get;
            }

            ISettingValue<LauncherPoints> LauncherAutologinPoints
            {
                get;
            }

            ISettingValue<bool> DatUpdaterEnabled
            {
                get;
            }

            ISettingValue<bool> UseCustomGw2Cache
            {
                get;
            }

            ISettingValue<bool> PreventDefaultCoherentUI
            {
                get;
            }

            ISettingValue<RelaunchOptions> PreventRelaunching
            {
                get;
            }
        }

        private class AccountTypeSettings : IAccountTypeSettings
        {
            public AccountTypeSettings()
            {
                _Path = new SettingValue<string>();
                _Arguments = new SettingValue<string>();
                _RunAfter = new SettingValue<RunAfter[]>();
                _Volume = new SettingValue<float>();
                _Mute = new SettingValue<MuteOptions>();
                _ScreenshotsFormat = new SettingValue<ScreenshotFormat>();
                _ScreenshotsLocation = new SettingValue<string>();
                _ProcessPriority = new SettingValue<ProcessPriorityClass>();
                _ProcessAffinity = new SettingValue<long>();
            }

            public SettingValue<string> _Path;
            public ISettingValue<string> Path
            {
                get
                {
                    return _Path;
                }
            }

            public SettingValue<string> _Arguments;
            public ISettingValue<string> Arguments
            {
                get
                {
                    return _Arguments;
                }
            }

            public SettingValue<RunAfter[]> _RunAfter;
            public ISettingValue<RunAfter[]> RunAfter
            {
                get
                {
                    return _RunAfter;
                }
            }

            public SettingValue<float> _Volume;
            public ISettingValue<float> Volume
            {
                get
                {
                    return _Volume;
                }
            }

            public SettingValue<MuteOptions> _Mute;
            public ISettingValue<MuteOptions> Mute
            {
                get
                {
                    return _Mute;
                }
            }

            public SettingValue<ScreenshotFormat> _ScreenshotsFormat;
            public ISettingValue<ScreenshotFormat> ScreenshotsFormat
            {
                get
                {
                    return _ScreenshotsFormat;
                }
            }

            public SettingValue<string> _ScreenshotsLocation;
            public ISettingValue<string> ScreenshotsLocation
            {
                get
                {
                    return _ScreenshotsLocation;
                }
            }

            public SettingValue<Settings.ProcessPriorityClass> _ProcessPriority;
            public ISettingValue<Settings.ProcessPriorityClass> ProcessPriority
            {
                get
                {
                    return _ProcessPriority;
                }
            }

            public SettingValue<long> _ProcessAffinity;
            public ISettingValue<long> ProcessAffinity
            {
                get
                {
                    return _ProcessAffinity;
                }
            }
        }

        private class Gw1Settings : AccountTypeSettings, IGw1Settings
        {

        }

        private class Gw2Settings : AccountTypeSettings, IGw2Settings
        {
            public Gw2Settings()
                : base()
            {
                _AutomaticRememberedLogin = new SettingValue<bool>();
                _ClientPort = new SettingValue<ushort>();
                _PrioritizeCoherentUI = new SettingValue<bool>();
                _MumbleLinkName = new SettingValue<string>();
                _VirtualUserPath = new PendingSettingValue<string>();
                _ProfileMode = new SettingValue<ProfileMode>();
                _ProfileOptions = new SettingValue<ProfileModeOptions>();
                _LocalizeAccountExecution = new PendingSettingValue<LocalizeAccountExecutionOptions>();
                _LauncherAutologinPoints = new SettingValue<LauncherPoints>();
                _DatUpdaterEnabled = new SettingValue<bool>();
                _UseCustomGw2Cache = new SettingValue<bool>();
                _PreventDefaultCoherentUI = new SettingValue<bool>();
                _PreventRelaunching = new SettingValue<RelaunchOptions>();
            }

            public SettingValue<bool> _AutomaticRememberedLogin;
            public ISettingValue<bool> AutomaticRememberedLogin
            {
                get
                {
                    return _AutomaticRememberedLogin;
                }
            }

            public SettingValue<ushort> _ClientPort;
            public ISettingValue<ushort> ClientPort
            {
                get
                {
                    return _ClientPort;
                }
            }

            public SettingValue<bool> _PrioritizeCoherentUI;
            public ISettingValue<bool> PrioritizeCoherentUI
            {
                get
                {
                    return _PrioritizeCoherentUI;
                }
            }

            public SettingValue<string> _MumbleLinkName;
            public ISettingValue<string> MumbleLinkName
            {
                get
                {
                    return _MumbleLinkName;
                }
            }

            public PendingSettingValue<string> _VirtualUserPath;
            public IPendingSettingValue<string> VirtualUserPath
            {
                get
                {
                    return _VirtualUserPath;
                }
            }

            public SettingValue<ProfileMode> _ProfileMode;
            public ISettingValue<ProfileMode> ProfileMode
            {
                get
                {
                    return _ProfileMode;
                }
            }

            public SettingValue<ProfileModeOptions> _ProfileOptions;
            public ISettingValue<ProfileModeOptions> ProfileOptions
            {
                get
                {
                    return _ProfileOptions;
                }
            }

            public PendingSettingValue<LocalizeAccountExecutionOptions> _LocalizeAccountExecution;
            public IPendingSettingValue<LocalizeAccountExecutionOptions> LocalizeAccountExecution
            {
                get
                {
                    return _LocalizeAccountExecution;
                }
            }

            public SettingValue<LauncherPoints> _LauncherAutologinPoints;
            public ISettingValue<LauncherPoints> LauncherAutologinPoints
            {
                get
                {
                    return _LauncherAutologinPoints;
                }
            }

            public SettingValue<bool> _DatUpdaterEnabled;
            public ISettingValue<bool> DatUpdaterEnabled
            {
                get
                {
                    return _DatUpdaterEnabled;
                }
            }

            public SettingValue<bool> _UseCustomGw2Cache;
            public ISettingValue<bool> UseCustomGw2Cache
            {
                get
                {
                    return _UseCustomGw2Cache;
                }
            }

            public SettingValue<bool> _PreventDefaultCoherentUI;
            public ISettingValue<bool> PreventDefaultCoherentUI
            {
                get
                {
                    return _PreventDefaultCoherentUI;
                }
            }

            public SettingValue<RelaunchOptions> _PreventRelaunching;
            public ISettingValue<RelaunchOptions> PreventRelaunching
            {
                get
                {
                    return _PreventRelaunching;
                }
            }
        }

        public class RunAfter : IEquatable<RunAfter>
        {
            [Flags]
            public enum RunAfterOptions : byte
            {
                None = 0,
                Enabled = 1,

                WaitForLauncherLoaded = 2,
                WaitForDxWindowLoaded = 4,

                CloseOnExit = 8,
                KillOnExit = 16,

                UseCurrentUser = 32,
            }

            public enum RunAfterType
            {
                ShellCommands = 1,
                Program = 2,
            }

            /// <summary>
            /// Program or commands to run after launching
            /// </summary>
            /// <param name="name">Name to display</param>
            /// <param name="path">Program path (not used for scripts)</param>
            /// <param name="args">Arguments or commands</param>
            /// <param name="flags">Options</param>
            public RunAfter(string name, string path, string args, RunAfterOptions flags)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    Name = name;
                }

                if (!string.IsNullOrEmpty(path))
                {
                    Path = path;
                }

                Arguments = args;
                Options = flags;
            }

            public RunAfterType Type
            {
                get
                {
                    if (Path == null)
                        return RunAfterType.ShellCommands;
                    return RunAfterType.Program;
                }
            }

            /// <summary>
            /// Name to display
            /// </summary>
            public string Name
            {
                get;
                protected set;
            }

            /// <summary>
            /// Program path
            /// </summary>
            public string Path
            {
                get;
                protected set;
            }

            /// <summary>
            /// Program arguments or commands
            /// </summary>
            public string Arguments
            {
                get;
                protected set;
            }

            public RunAfterOptions Options
            {
                get;
                protected set;
            }

            public bool Equals(RunAfter ra)
            {
                return ra.Options == this.Options && ra.Arguments == this.Arguments && ra.Path == this.Path && ra.Name == this.Name;
            }

            /// <summary>
            /// Returns a valid name
            /// </summary>
            public string GetName(string defaultName = null)
            {
                if (string.IsNullOrEmpty(this.Name))
                {
                    if (this.Type == Settings.RunAfter.RunAfterType.Program)
                    {
                        try
                        {
                            return System.IO.Path.GetFileName(this.Path);
                        }
                        catch
                        {
                            if (string.IsNullOrEmpty(this.Path))
                            {
                                return defaultName == null ? "Program" : defaultName;
                            }
                            else
                                return this.Path;
                        }
                    }
                    else
                    {
                        return defaultName == null ? "cmd.exe" : defaultName;
                    }
                }
                else
                {
                    return this.Name;
                }
            }

            /// <summary>
            /// Returns a path based on the type
            /// </summary>
            public string GetPath()
            {
                if (Path == null)
                {
                    return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe");
                }

                return Path;
            }
        }

        #endregion

        private static object _lock = new object();
        private static System.Threading.CancellationTokenSource cancelWrite;
        private static Task task;
        private static DateTime _lastModified;
        private static SettingsFile _settingsFile;
        private static ushort _accountUID;
        private static ushort _datUID;
        private static ushort _gfxUID;
        private static ushort _gwdatUID;

        static Settings()
        {
            HEADER = new byte[] { 41, 229, 122, 91, 23 };

            cancelWrite = new System.Threading.CancellationTokenSource();
            _settingsFile = new SettingsFile();

            FORMS = new Type[]
            {
                typeof(UI.formMain),
                typeof(UI.formDailies),
                typeof(UI.formNotes),
                typeof(UI.formAccountBar),
                typeof(UI.formSettings),
            };

            _WindowBounds = new KeyedProperty<Type, Rectangle>();
            _Accounts = new KeyedProperty<ushort, IAccount>();
            _ShowTray = new SettingValue<bool>();
            _MinimizeToTray = new SettingValue<bool>();
            _BringToFrontOnExit = new SettingValue<bool>();
            _StoreCredentials = new SettingValue<bool>();
            _DeleteCacheOnLaunch = new SettingValue<bool>();
            _HiddenUserAccounts = new KeyedProperty<string, bool>(StringComparer.OrdinalIgnoreCase);
            _LastKnownBuild = new SettingValue<int>();
            _FontStatus = new SettingValue<Font>();
            _FontName = new SettingValue<Font>();
            _StyleShowAccount = new SettingValue<bool>();
            _DatFiles = new KeyedProperty<ushort, IDatFile>(
                new Func<ushort, ISettingValue<IDatFile>>(
                    delegate(ushort key)
                    {
                        return new SettingValue<IDatFile>(new DatFile(key));
                    }));
            _GfxFiles = new KeyedProperty<ushort, IGfxFile>(
                new Func<ushort, ISettingValue<IGfxFile>>(
                    delegate(ushort key)
                    {
                        return new SettingValue<IGfxFile>(new GfxFile(key));
                    }));
            _LocalAssetServerEnabled = new SettingValue<bool>();
            _BackgroundPatchingEnabled = new SettingValue<bool>();
            _BackgroundPatchingNotifications = new SettingValue<ScreenAttachment>();
            _BackgroundPatchingLang = new SettingValue<byte>();
            _BackgroundPatchingMaximumThreads = new SettingValue<byte>();
            _PatchingSpeedLimit = new SettingValue<int>();
            _AutoUpdate = new SettingValue<bool>();
            _AutoUpdateInterval = new SettingValue<ushort>();
            _LastProgramVersion = new SettingValue<LastCheckedVersion>();
            _CheckForNewBuilds = new SettingValue<bool>();
            _PreventTaskbarGrouping = new SettingValue<bool>();
            _WindowCaption = new SettingValue<string>();

            _TopMost = new SettingValue<bool>();
            _ActionActiveLClick = new SettingValue<ButtonAction>();
            _ActionActiveLPress = new SettingValue<ButtonAction>();
            _ShowDailies = new SettingValue<DailiesMode>();
            _HiddenDailyCategories = new KeyedProperty<byte, bool>();
            _BackgroundPatchingProgress = new SettingValue<Rectangle>();
            _NetworkAuthorization = new SettingValue<NetworkAuthorizationFlags>();
            _ScreenshotNaming = new SettingValue<string>();
            _ScreenshotConversion = new SettingValue<ScreenshotConversionOptions>();
            _DeleteCrashLogsOnLaunch = new SettingValue<bool>();
            _NotesNotifications = new SettingValue<NotificationScreenAttachment>();
            _MaxPatchConnections = new SettingValue<byte>();

            _ActionInactiveLClick = new SettingValue<ButtonAction>();
            _StyleShowColor = new SettingValue<bool>();
            _StyleHighlightFocused = new SettingValue<bool>();
            _WindowIcon = new SettingValue<bool>();
            _AccountBarEnabled = new SettingValue<bool>();
            _AccountBarStyle = new SettingValue<AccountBarStyles>();
            _AccountBarOptions = new SettingValue<AccountBarOptions>();
            _AccountBarSorting = new SettingValue<SortingOptions>();
            _AccountBarDocked = new SettingValue<ScreenAnchor>();
            _UseDefaultIconForShortcuts = new SettingValue<bool>();
            _LimitActiveAccounts = new SettingValue<byte>();
            _DelayLaunchUntilLoaded = new SettingValue<bool>();
            _DelayLaunchSeconds = new SettingValue<byte>();

            _ShowKillAllAccounts = new SettingValue<bool>();

            _RepaintInitialWindow = new SettingValue<bool>();

            _GwDatFiles = new KeyedProperty<ushort, IGwDatFile>(
                new Func<ushort, ISettingValue<IGwDatFile>>(
                    delegate(ushort key)
                    {
                        return new SettingValue<IGwDatFile>(new GwDatFile(key));
                    }));

            _GuildWars1 = new Gw1Settings();
            _GuildWars2 = new Gw2Settings();

            _Encryption = new SettingValue<EncryptionOptions>();
            _ScreenshotNotifications = new SettingValue<ScreenAttachment>();
            _PatchingOptions = new SettingValue<PatchingFlags>();
            _PatchingPort = new SettingValue<ushort>();
            _FontUser = new SettingValue<Font>();
            _StyleShowIcon = new SettingValue<bool>();
            _StyleColumns = new SettingValue<byte>();
            _Sorting = new SettingValue<SortingOptions>();
            _StyleBackgroundImage = new SettingValue<string>();
            _StyleColors = new SettingValue<AccountGridButtonColors>();
            _WindowTemplates = new ListProperty<WindowTemplate>();
            _LaunchBehindOtherAccounts = new SettingValue<bool>();
            _LaunchLimiter = new SettingValue<LaunchLimiterOptions>();
            _ShowLaunchAllAccounts = new SettingValue<bool>();
            _LaunchTimeout = new SettingValue<byte>();
            _SelectedPage = new SettingValue<byte>();
            _JumpList = new SettingValue<JumpListOptions>();
            _PreventTaskbarMinimize = new SettingValue<bool>();
            _ActionActiveMClick = new SettingValue<ButtonAction>();
            _ActionInactiveMClick = new SettingValue<ButtonAction>();
            _AuthenticatorPastingEnabled = new SettingValue<bool>();
            _ProcessPriority = new SettingValue<ProcessPriorityClass>();
            _PublicIPAddress = new SettingValue<byte[]>();
        }

        public static void Load()
        {
            var path = _settingsFile.Path;
            var temp = _settingsFile.Temp;

            try
            {
                var tmp = new FileInfo(temp);
                ushort version;

                if (tmp.Exists && tmp.Length > 0)
                {
                    try
                    {
                        version = Load(temp);
                        try
                        {
                            if (File.Exists(path))
                                File.Delete(path);
                            File.Move(temp, path);
                        }
                        catch (Exception ex)
                        {
                            Util.Logging.Log(ex);
                        }
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        System.Diagnostics.Debugger.Break();
#endif
                        Util.Logging.Log(ex);
                        version = Load(path);
                    }
                }
                else
                    version = Load(path);

                _settingsFile.Loaded(version);
            }
            catch (Exception e)
            {
#if DEBUG
                System.Diagnostics.Debugger.Break();
#endif

                Util.Logging.Log(e);

                _settingsFile.Loaded();

                var failed = true;

                foreach (var p in _settingsFile.GetBackups())
                {
                    try
                    {
                        Load(p);
                        failed = false;
                        break;
                    }
                    catch { }
                }

                if (failed)
                    SetDefaults();
            }
        }

        public static void SetDefaults()
        {
            _StoreCredentials.SetValue(true);
            _ShowTray.SetValue(true);
        }

        private static void OnValueChanged()
        {
            DelayedWrite();
        }

        private static void DelayedWrite()
        {
            lock (_lock)
            {
                _lastModified = DateTime.UtcNow;
                if (task == null || task.IsCompleted)
                {
                    var cancel = cancelWrite.Token;

                    task = new Task(
                        delegate
                        {
                            DelayedWrite(cancel);
                        });
                    task.Start();
                }
            }
        }

        private static void DelayedWrite(System.Threading.CancellationToken cancel)
        {
            while (true)
            {
                while (true)
                {
                    int remaining = WRITE_DELAY - (int)DateTime.UtcNow.Subtract(_lastModified).TotalMilliseconds;
                    if (remaining > 0)
                    {
                        if (cancel.WaitHandle.WaitOne(remaining))
                        {
                            _lastModified = DateTime.MinValue;
                            break;
                        }
                    }
                    else
                    {
                        lock (_lock)
                        {
                            remaining = WRITE_DELAY - (int)DateTime.UtcNow.Subtract(_lastModified).TotalMilliseconds;
                            if (remaining <= 0)
                            {
                                _lastModified = DateTime.MinValue;
                                break;
                            }
                        }
                    }
                }

                var e = Write();

                if (cancel.IsCancellationRequested)
                    return;

                lock (_lock)
                {
                    if (e != null || _lastModified != DateTime.MinValue)
                    {
                        _lastModified = DateTime.UtcNow;
                    }
                    else
                    {
                        task = null;
                        return;
                    }
                }
            }
        }

        public static byte[] CompressBooleans(params bool[] b)
        {
            if (b.Length == 0)
                return new byte[0];

            byte[] bytes = new byte[(b.Length - 1) / 8 + 1];
            int p = 0;
            byte bit = 7;

            for (int i = 0; i < b.Length; i++)
            {
                if (b[i])
                {
                    bytes[p] |= (byte)(1 << bit);
                }

                if (bit == 0)
                {
                    bit = 7;
                    p++;
                }
                else
                    bit--;
            }

            return bytes;
        }

        public static bool[] ExpandBooleans(byte[] bytes)
        {
            bool[] bools = new bool[bytes.Length * 8];

            for (int i = 0; i < bytes.Length; i++)
            {
                var b = bytes[i];
                if (b > 0)
                {
                    int p = i * 8;
                    for (int j = 0; j < 8; j++)
                    {
                        bools[p + j] = (b >> 7 - j & 1) == 1;
                    }
                }
            }

            return bools;
        }

        private static void WriteVariableLength(BinaryWriter writer, int n)
        {
            if (n < byte.MaxValue)
            {
                writer.Write((byte)n);
            }
            else if (n < ushort.MaxValue)
            {
                writer.Write(byte.MaxValue);
                writer.Write((ushort)n);
            }
            else
            {
                writer.Write(byte.MaxValue);
                writer.Write(ushort.MaxValue);
                writer.Write(n);
            }
        }

        private static int ReadVariableLength(BinaryReader reader)
        {
            int n = reader.ReadByte();
            if (n == byte.MaxValue)
            {
                n = reader.ReadUInt16();
                if (n == ushort.MaxValue)
                {
                    n = reader.ReadInt32();
                }
            }
            return n;
        }

        private static void Write(BinaryWriter writer, Rectangle r)
        {
            writer.Write(r.X);
            writer.Write(r.Y);
            writer.Write(r.Width);
            writer.Write(r.Height);
        }

        private static Rectangle ReadRectangle(BinaryReader reader)
        {
            return new Rectangle(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
        }

        private static Exception Write()
        {
            if (ReadOnly)
                return null;

            try
            {
                IO.CrcStream crc;

                using (var writer = new BinaryWriter(crc = new IO.CrcStream(new BufferedStream(File.Open(_settingsFile.Temp, FileMode.Create, FileAccess.Write, FileShare.Read)), false)))
                {
                    writer.Write(HEADER);
                    writer.Write(VERSION);

                    bool[] booleans;

                    lock (_WindowBounds)
                    {
                        var count = _WindowBounds.Count;
                        var items = new KeyValuePair<Type, Rectangle>[count];
                        int i = 0;

                        foreach (var key in _WindowBounds.Keys)
                        {
                            if (i == count)
                                break;
                            var o = _WindowBounds[key];
                            if (o.HasValue)
                            {
                                items[i++] = new KeyValuePair<Type, Rectangle>(key, o.Value);
                            }
                        }

                        count = i;

                        writer.Write((byte)count);

                        for (i = 0; i < count; i++)
                        {
                            var item = items[i];

                            writer.Write(GetWindowID(item.Key));

                            writer.Write(item.Value.X);
                            writer.Write(item.Value.Y);
                            writer.Write(item.Value.Width);
                            writer.Write(item.Value.Height);
                        }
                    }

                    booleans = new bool[]
                    {
                        //0
                        _Sorting.HasValue,
                        _Encryption.HasValue,
                        _StoreCredentials.HasValue,
                        _ShowTray.HasValue,
                        _MinimizeToTray.HasValue,
                        _BringToFrontOnExit.HasValue,
                        _DeleteCacheOnLaunch.HasValue,
                        _GuildWars2._Path.HasValue && !string.IsNullOrWhiteSpace(_GuildWars2._Path.Value),
                        //8
                        _GuildWars2._Arguments.HasValue && !string.IsNullOrWhiteSpace(_GuildWars2._Arguments.Value),
                        _LastKnownBuild.HasValue,
                        _FontName.HasValue,
                        _FontStatus.HasValue,
                        _StyleShowAccount.HasValue,
                        _StoreCredentials.Value,
                        _ShowTray.Value,
                        _MinimizeToTray.Value,
                        //16
                        _BringToFrontOnExit.Value,
                        _DeleteCacheOnLaunch.Value,
                        _StyleShowAccount.Value,
                        _CheckForNewBuilds.HasValue,
                        _LastProgramVersion.HasValue,
                        _AutoUpdateInterval.HasValue,
                        _AutoUpdate.HasValue,
                        _BackgroundPatchingEnabled.HasValue,
                        //24
                        _BackgroundPatchingLang.HasValue,
                        _BackgroundPatchingNotifications.HasValue,
                        _BackgroundPatchingMaximumThreads.HasValue,
                        _PatchingSpeedLimit.HasValue,
                        _PatchingOptions.HasValue,
                        _GuildWars2._RunAfter.HasValue,
                        _GuildWars2._Volume.HasValue,
                        _LocalAssetServerEnabled.HasValue,
                        //32
                        _CheckForNewBuilds.Value,
                        _AutoUpdate.Value,
                        _BackgroundPatchingEnabled.Value,
                        _LocalAssetServerEnabled.Value,
                        _PatchingPort.HasValue,
                        _WindowCaption.HasValue && !string.IsNullOrWhiteSpace(_WindowCaption.Value),
                        _PreventTaskbarGrouping.HasValue,
                        _PreventTaskbarGrouping.Value,
                        //40
                        _GuildWars2._AutomaticRememberedLogin.HasValue,
                        _GuildWars2._Mute.HasValue,
                        _GuildWars2._ClientPort.HasValue,
                        _GuildWars2._ScreenshotsFormat.HasValue,
                        _GuildWars2._ScreenshotsLocation.HasValue,
                        _TopMost.HasValue,
                        _GuildWars2._VirtualUserPath.HasValue,
                        _GuildWars2._VirtualUserPath.IsPending,
                        //48
                        _ActionActiveLClick.HasValue,
                        _ActionActiveLPress.HasValue,
                        _ShowDailies.HasValue,
                        _BackgroundPatchingProgress.HasValue,
                        _NetworkAuthorization.HasValue,
                        _ScreenshotNaming.HasValue,
                        _ScreenshotConversion.HasValue,
                        _DeleteCrashLogsOnLaunch.HasValue,
                        //56
                        _GuildWars2._ProcessPriority.HasValue,
                        _GuildWars2._ProcessAffinity.HasValue,
                        _GuildWars2._PrioritizeCoherentUI.HasValue,
                        _NotesNotifications.HasValue,
                        _MaxPatchConnections.HasValue,
                        _GuildWars2._AutomaticRememberedLogin.Value,
                        _TopMost.Value,
                        _DeleteCrashLogsOnLaunch.Value,
                        //64
                        _GuildWars2._PrioritizeCoherentUI.Value,
                        _ActionInactiveLClick.HasValue,
                        _StyleShowColor.HasValue,
                        _StyleHighlightFocused.HasValue,
                        _WindowIcon.HasValue,
                        _AccountBarEnabled.HasValue,
                        _AccountBarStyle.HasValue,
                        _AccountBarOptions.HasValue,
                        //72
                        _AccountBarSorting.HasValue,
                        _ScreenshotNotifications.HasValue,
                        _AccountBarDocked.HasValue,
                        _UseDefaultIconForShortcuts.HasValue,
                        _LimitActiveAccounts.HasValue,
                        _DelayLaunchUntilLoaded.HasValue,
                        _DelayLaunchSeconds.HasValue,
                        _GuildWars2._LocalizeAccountExecution.HasValue,
                        //80
                        _GuildWars2._LocalizeAccountExecution.IsPending,
                        _GuildWars2._LauncherAutologinPoints.HasValue,
                        _StyleShowColor.Value,
                        _StyleHighlightFocused.Value,
                        _WindowIcon.Value,
                        _AccountBarEnabled.Value,
                        _UseDefaultIconForShortcuts.Value,
                        _DelayLaunchUntilLoaded.Value,
                        //88
                        _GuildWars2._DatUpdaterEnabled.HasValue,
                        _GuildWars2._UseCustomGw2Cache.HasValue,
                        _ShowKillAllAccounts.HasValue,
                        _GuildWars2._PreventDefaultCoherentUI.HasValue,
                        _GuildWars2._DatUpdaterEnabled.Value,
                        _GuildWars2._UseCustomGw2Cache.Value,
                        _ShowKillAllAccounts.Value,
                        _GuildWars2._PreventDefaultCoherentUI.Value,
                        //96
                        _RepaintInitialWindow.HasValue,
                        _RepaintInitialWindow.Value,
                        _GuildWars2._MumbleLinkName.HasValue,
                        _GuildWars2._ProfileMode.HasValue,
                        _GuildWars2._ProfileOptions.HasValue,
                        _FontUser.HasValue,
                        _StyleColumns.HasValue,
                        _StyleShowIcon.HasValue,
                        //104
                        _StyleShowIcon.Value,
                        _StyleBackgroundImage.HasValue,
                        _StyleColors.HasValue,
                        _GuildWars1._Path.HasValue && !string.IsNullOrWhiteSpace(_GuildWars1._Path.Value),
                        _GuildWars1._Arguments.HasValue && !string.IsNullOrWhiteSpace(_GuildWars1._Arguments.Value),
                        _GuildWars1._Mute.HasValue,
                        _GuildWars1._ProcessAffinity.HasValue,
                        _GuildWars1._ProcessPriority.HasValue,
                        //112
                        _GuildWars1._RunAfter.HasValue,
                        _GuildWars1._ScreenshotsFormat.HasValue,
                        _GuildWars1._ScreenshotsLocation.HasValue,
                        _GuildWars1._Volume.HasValue,
                        _WindowTemplates.Count > 0,
                        _LaunchBehindOtherAccounts.HasValue,
                        _LaunchBehindOtherAccounts.Value,
                        _LaunchLimiter.HasValue,
                        //120
                        _ShowLaunchAllAccounts.HasValue,
                        _ShowLaunchAllAccounts.Value,
                        _LaunchTimeout.HasValue,
                        _SelectedPage.Value > 0,
                        _JumpList.HasValue,
                        _PreventTaskbarMinimize.HasValue,
                        _PreventTaskbarMinimize.Value,
                        _GuildWars2._PreventRelaunching.HasValue,
                        //128
                        _ActionActiveMClick.HasValue,
                        _ActionInactiveMClick.HasValue,
                        _AuthenticatorPastingEnabled.HasValue,
                        _AuthenticatorPastingEnabled.Value,
                        _ProcessPriority.HasValue,
                        _PublicIPAddress.HasValue && _NetworkAuthorization.Value.HasFlag(Settings.NetworkAuthorizationFlags.VerifyIP),
                        //134
                    };

                    byte[] b = CompressBooleans(booleans);

                    writer.Write((byte)b.Length);
                    writer.Write(b);

                    if (booleans[0])
                        writer.Write(_Sorting.Value.ToBytes());
                    if (booleans[1])
                    {
                        var scope = _Encryption.Value.Scope;
                        var key = _Encryption.Value.Key;

                        writer.Write((byte)scope);

                        if (key != null)
                        {
                            writer.Write((byte)key.Length);
                            writer.Write(key);
                        }
                        else
                        {
                            writer.Write((byte)0);
                        }
                    }
                    if (booleans[7])
                        writer.Write(_GuildWars2._Path.Value);
                    if (booleans[8])
                        writer.Write(_GuildWars2._Arguments.Value);
                    if (booleans[9])
                        writer.Write(_LastKnownBuild.Value);
                    if (booleans[10])
                    {
                        try
                        {
                            writer.Write(new FontConverter().ConvertToString(_FontName.Value));
                        }
                        catch (Exception e)
                        {
                            Util.Logging.Log(e);
                            writer.Write("");
                        }
                    }
                    if (booleans[11])
                    {
                        try
                        {
                            writer.Write(new FontConverter().ConvertToString(_FontStatus.Value));
                        }
                        catch (Exception e)
                        {
                            Util.Logging.Log(e);
                            writer.Write("");
                        }
                    }

                    //v2
                    if (booleans[20])
                    {
                        var v = _LastProgramVersion.Value;
                        writer.Write(v.LastCheck.ToBinary());
                        writer.Write(v.Version);
                    }
                    if (booleans[21])
                        writer.Write(_AutoUpdateInterval.Value);
                    if (booleans[24])
                        writer.Write(_BackgroundPatchingLang.Value);
                    if (booleans[25])
                    {
                        var v = _BackgroundPatchingNotifications.Value;
                        writer.Write((byte)v.Screen);
                        writer.Write((byte)v.Anchor);
                    }
                    if (booleans[26])
                        writer.Write(_BackgroundPatchingMaximumThreads.Value);
                    if (booleans[27])
                        writer.Write(_PatchingSpeedLimit.Value);
                    if (booleans[28])
                        writer.Write((byte)_PatchingOptions.Value);
                    if (booleans[29])
                    {
                        var ra = _GuildWars2._RunAfter.Value;
                        WriteVariableLength(writer, ra.Length);
                        foreach (var r in ra)
                        {
                            writer.Write(r.Name == null ? "" : r.Name);
                            writer.Write(r.Path == null ? "" : r.Path);
                            writer.Write(r.Arguments);
                            writer.Write((byte)r.Options);
                        }
                    }
                    if (booleans[30])
                        writer.Write((byte)(_GuildWars2._Volume.Value * 255));

                    if (booleans[36])
                        writer.Write(_PatchingPort.Value);

                    //v3
                    if (booleans[37])
                        writer.Write(_WindowCaption.Value);

                    //v4
                    if (booleans[41])
                        writer.Write((byte)_GuildWars2._Mute.Value);
                    if (booleans[42])
                        writer.Write(_GuildWars2._ClientPort.Value);
                    if (booleans[43])
                        writer.Write((byte)_GuildWars2._ScreenshotsFormat.Value);
                    if (booleans[44])
                        writer.Write(_GuildWars2._ScreenshotsLocation.Value);
                    if (booleans[46])
                        writer.Write(_GuildWars2._VirtualUserPath.Value);
                    if (booleans[47])
                    {
                        if (_GuildWars2._VirtualUserPath.ValueCommit == null)
                            writer.Write(string.Empty);
                        else
                            writer.Write(_GuildWars2._VirtualUserPath.ValueCommit);
                    }
                    if (booleans[48])
                        writer.Write((byte)_ActionActiveLClick.Value);
                    if (booleans[49])
                        writer.Write((byte)_ActionActiveLPress.Value);
                    if (booleans[50])
                        writer.Write((byte)_ShowDailies.Value);
                    if (booleans[51])
                    {
                        var r = _BackgroundPatchingProgress.Value;
                        writer.Write(r.X);
                        writer.Write(r.Y);
                        writer.Write(r.Width);
                        writer.Write(r.Height);
                    }
                    if (booleans[52])
                        writer.Write((byte)_NetworkAuthorization.Value);
                    if (booleans[53])
                        writer.Write(_ScreenshotNaming.Value);
                    if (booleans[54])
                    {
                        var v = _ScreenshotConversion.Value;
                        writer.Write((byte)v.Format);
                        writer.Write(v.raw);
                    }
                    if (booleans[56])
                        writer.Write((byte)_GuildWars2._ProcessPriority.Value);
                    if (booleans[57])
                        writer.Write(_GuildWars2._ProcessAffinity.Value);
                    if (booleans[59])
                    {
                        var v = _NotesNotifications.Value;
                        writer.Write((byte)v.Screen);
                        writer.Write((byte)v.Anchor);
                        writer.Write(v.OnlyWhileActive);
                    }
                    if (booleans[60])
                        writer.Write(_MaxPatchConnections.Value);

                    //v5
                    if (booleans[65])
                        writer.Write((byte)_ActionInactiveLClick.Value);
                    if (booleans[70])
                        writer.Write((byte)_AccountBarStyle.Value);
                    if (booleans[71])
                        writer.Write((byte)_AccountBarOptions.Value);
                    if (booleans[72])
                        writer.Write(_AccountBarSorting.Value.ToBytes());
                    if (booleans[73])
                    {
                        var v = _ScreenshotNotifications.Value;
                        writer.Write((byte)v.Screen);
                        writer.Write((byte)v.Anchor);
                    }
                    if (booleans[74])
                        writer.Write((byte)_AccountBarDocked.Value);
                    if (booleans[76])
                        writer.Write(_LimitActiveAccounts.Value);
                    if (booleans[78])
                        writer.Write(_DelayLaunchSeconds.Value);
                    if (booleans[79])
                        writer.Write((byte)_GuildWars2._LocalizeAccountExecution.Value);
                    if (booleans[80])
                        writer.Write((byte)_GuildWars2._LocalizeAccountExecution.ValueCommit);
                    if (booleans[81])
                    {
                        var v = _GuildWars2._LauncherAutologinPoints.Value;
                        writer.Write(v.EmptyArea.X);
                        writer.Write(v.EmptyArea.Y);
                        writer.Write(v.PlayButton.X);
                        writer.Write(v.PlayButton.Y);
                    }
                    if (booleans[98])
                        writer.Write(_GuildWars2._MumbleLinkName.Value);
                    if (booleans[99])
                        writer.Write((byte)_GuildWars2._ProfileMode.Value);
                    if (booleans[100])
                        writer.Write((byte)_GuildWars2._ProfileOptions.Value);
                    if (booleans[101])
                    {
                        try
                        {
                            writer.Write(new FontConverter().ConvertToString(_FontUser.Value));
                        }
                        catch (Exception e)
                        {
                            Util.Logging.Log(e);
                            writer.Write("");
                        }
                    }
                    if (booleans[102])
                        writer.Write(_StyleColumns.Value);
                    if (booleans[105])
                        writer.Write(_StyleBackgroundImage.Value);
                    if (booleans[106])
                    {
                        var colors = _StyleColors.Value.Values;
                        writer.Write((byte)colors.Length);
                        foreach (var c in colors)
                        {
                            writer.Write(c.ToArgb());
                        }
                    }
                    if (booleans[107])
                        writer.Write(_GuildWars1._Path.Value);
                    if (booleans[108])
                        writer.Write(_GuildWars1._Arguments.Value);
                    if (booleans[109])
                        writer.Write((byte)_GuildWars1._Mute.Value);
                    if (booleans[110])
                        writer.Write(_GuildWars1._ProcessAffinity.Value);
                    if (booleans[111])
                        writer.Write((byte)_GuildWars1._ProcessPriority.Value);
                    if (booleans[112])
                    {
                        var ra = _GuildWars1._RunAfter.Value;
                        WriteVariableLength(writer, ra.Length);
                        foreach (var r in ra)
                        {
                            writer.Write(r.Name == null ? "" : r.Name);
                            writer.Write(r.Path == null ? "" : r.Path);
                            writer.Write(r.Arguments);
                            writer.Write((byte)r.Options);
                        }
                    }
                    if (booleans[113])
                        writer.Write((byte)_GuildWars1.ScreenshotsFormat.Value);
                    if (booleans[114])
                        writer.Write(_GuildWars1.ScreenshotsLocation.Value);
                    if (booleans[115])
                        writer.Write((byte)(_GuildWars1._Volume.Value * 255));
                    if (booleans[116])
                    {
                        lock (_WindowTemplates)
                        {
                            var wt = _WindowTemplates;
                            var count = _WindowTemplates.Count;

                            WriteVariableLength(writer, count);

                            for (var i = 0; i < count; i++)
                            {
                                WriteVariableLength(writer, wt[i].Screens.Length);

                                foreach (var screen in wt[i].Screens)
                                {
                                    Write(writer, screen.Bounds);
                                    WriteVariableLength(writer, screen.Windows.Length);

                                    foreach (var rect in screen.Windows)
                                    {
                                        Write(writer, rect);
                                    }
                                }
                            }
                        }
                    }
                    if (booleans[119])
                    {
                        var v = _LaunchLimiter.Value;
                        writer.Write(v.Count);
                        writer.Write(v.RechargeCount);
                        writer.Write(v.RechargeTime);
                    }
                    if (booleans[122])
                        writer.Write(_LaunchTimeout.Value);

                    if (booleans[123])
                        writer.Write(_SelectedPage.Value);

                    if (booleans[124])
                        writer.Write((byte)_JumpList.Value);

                    if (booleans[127])
                        writer.Write((byte)_GuildWars2._PreventRelaunching.Value);

                    if (booleans[128])
                        writer.Write((byte)_ActionActiveMClick.Value);

                    if (booleans[129])
                        writer.Write((byte)_ActionInactiveMClick.Value);

                    if (booleans[132])
                        writer.Write((byte)_ProcessPriority.Value);

                    if (booleans[133])
                    {
                        var bytes = _PublicIPAddress.Value;
                        if (bytes == null || bytes.Length == 0)
                        {
                            writer.Write((byte)0);
                        }
                        else
                        {
                            writer.Write((byte)bytes.Length);
                            writer.Write(bytes);
                        }
                    }

                    lock (_DatFiles)
                    {
                        var count = _DatFiles.Count;
                        var items = new KeyValuePair<ushort, DatFile>[count];
                        int i = 0;

                        foreach (var key in _DatFiles.Keys)
                        {
                            if (i == count)
                                break;
                            var o = _DatFiles[key];
                            if (o.HasValue && ((DatFile)o.Value).ReferenceCount > 0)
                            {
                                items[i++] = new KeyValuePair<ushort, DatFile>(key, (DatFile)o.Value);
                            }
                        }

                        count = i;

                        writer.Write((ushort)count);

                        for (i = 0; i < count; i++)
                        {
                            var item = items[i];

                            writer.Write(item.Value.UID);
                            if (string.IsNullOrWhiteSpace(item.Value.Path))
                                writer.Write("");
                            else
                                writer.Write(item.Value.Path);
                            //writer.Write(item.Value.IsInitialized); //v6
                            writer.Write((byte)item.Value._flags);
                        }
                    }

                    //v4
                    lock (_GfxFiles)
                    {
                        var count = _GfxFiles.Count;
                        var items = new KeyValuePair<ushort, GfxFile>[count];
                        int i = 0;

                        foreach (var key in _GfxFiles.Keys)
                        {
                            if (i == count)
                                break;
                            var o = _GfxFiles[key];
                            if (o.HasValue && ((GfxFile)o.Value).ReferenceCount > 0)
                            {
                                items[i++] = new KeyValuePair<ushort, GfxFile>(key, (GfxFile)o.Value);
                            }
                        }

                        count = i;

                        writer.Write((ushort)count);

                        for (i = 0; i < count; i++)
                        {
                            var item = items[i];

                            writer.Write(item.Value.UID);
                            if (string.IsNullOrWhiteSpace(item.Value.Path))
                                writer.Write("");
                            else
                                writer.Write(item.Value.Path);
                            //writer.Write(item.Value.IsInitialized); //v6
                            writer.Write((byte)item.Value._flags);
                        }
                    }

                    //v10
                    lock (_GwDatFiles)
                    {
                        var count = _GwDatFiles.Count;
                        var items = new KeyValuePair<ushort, GwDatFile>[count];
                        int i = 0;

                        foreach (var key in _GwDatFiles.Keys)
                        {
                            if (i == count)
                                break;
                            var o = _GwDatFiles[key];
                            if (o.HasValue && ((GwDatFile)o.Value).ReferenceCount > 0)
                            {
                                items[i++] = new KeyValuePair<ushort, GwDatFile>(key, (GwDatFile)o.Value);
                            }
                        }

                        count = i;

                        writer.Write((ushort)count);

                        for (i = 0; i < count; i++)
                        {
                            var item = items[i];

                            writer.Write(item.Value.UID);
                            if (string.IsNullOrWhiteSpace(item.Value.Path))
                                writer.Write("");
                            else
                                writer.Write(item.Value.Path);
                            writer.Write((byte)item.Value._flags);
                        }
                    }

                    lock (_Accounts)
                    {
                        var count = _Accounts.Count;
                        var items = new KeyValuePair<ushort, Account>[count];
                        int i = 0;

                        foreach (var key in _Accounts.Keys)
                        {
                            if (i == count)
                                break;
                            var o = _Accounts[key];
                            if (o.HasValue)
                            {
                                items[i++] = new KeyValuePair<ushort, Account>(key, (Account)o.Value);
                            }
                        }

                        count = i;

                        Security.Cryptography.Crypto crypto = null;

                        try
                        {
                            writer.Write((ushort)count);

                            for (i = 0; i < count; i++)
                            {
                                var item = items[i];

                                var account = item.Value;

                                writer.Write((byte)account._Type);
                                writer.Write(account._UID);
                                writer.Write(account._Name);
                                writer.Write(account._WindowsAccount != null ? account._WindowsAccount : "");
                                writer.Write(account._CreatedUtc.ToBinary());
                                writer.Write(account._LastUsedUtc.ToBinary());
                                writer.Write(account._TotalUses);
                                writer.Write(account._Arguments != null ? account._Arguments : "");

                                booleans = new bool[]
                                {
                                    //0
                                    account._ShowDailyLogin,
                                    account.Windowed,
                                    account._RecordLaunches,
                                    account._AutologinOptions != AutologinOptions.None,
                                    account._JumpListPinning != JumpListPinning.None,
                                    account.VolumeEnabled,
                                    account._RunAfter != null,
                                    !string.IsNullOrEmpty(account._Email),
                                    account._Password != null && !account._Password.Data.IsEmpty,
                                    !string.IsNullOrEmpty(account._BackgroundImage),
                                    //10
                                    account._Image != null,
                                    account._PendingFiles,
                                    !string.IsNullOrEmpty(account._ScreenshotsLocation),
                                    account._Pages != null, //!string.IsNullOrEmpty(account._ApiKey),
                                    account._TotpKey != null && account._TotpKey.Length > 0,
                                    false, //account._ApiData != null,
                                    !account._WindowBounds.IsEmpty,
                                    false, //(byte)account._ProcessPriority > 0,
                                    false, //account._ClientPort != 0,
                                    false, //account._LastDailyCompletionUtc.Ticks != 1,
                                    //20
                                    account._Mute  != 0,
                                    account._ScreenshotsFormat != 0,
                                    account._NetworkAuthorizationState != NetworkAuthorizationState.Disabled,
                                    account._ProcessPriority != ProcessPriorityClass.None,
                                    account._ProcessAffinity != 0,
                                    account._Notes != null && account._Notes.Count > 0,
                                    !account._ColorKey.IsEmpty,
                                    account._IconType != IconType.None,
                                    account._SortKey != (i + 1),
                                    false, //account._AutomaticPlay
                                    //30
                                };

                                b = CompressBooleans(booleans);

                                writer.Write((byte)b.Length);
                                writer.Write(b);

                                if (booleans[1])
                                    writer.Write((byte)account._WindowOptions);

                                if (booleans[3])
                                    writer.Write((byte)account._AutologinOptions);

                                if (booleans[4])
                                    writer.Write((byte)account._JumpListPinning);

                                if (booleans[5])
                                    writer.Write(account._Volume);

                                if (booleans[6])
                                {
                                    var ra = account._RunAfter;
                                    WriteVariableLength(writer, ra.Length);
                                    foreach (var r in ra)
                                    {
                                        writer.Write(r.Name == null ? "" : r.Name);
                                        writer.Write(r.Path == null ? "" : r.Path);
                                        writer.Write(r.Arguments);
                                        writer.Write((byte)r.Options);
                                    }
                                }

                                //v4
                                if (booleans[7])
                                    writer.Write(account._Email);

                                if (booleans[8])
                                {
                                    byte[] data;
                                    try
                                    {
                                        if (crypto == null)
                                        {
                                            var o = _Encryption.Value;
                                            crypto = new Security.Cryptography.Crypto(o != null ? o.Scope : EncryptionScope.CurrentUser, Security.Cryptography.Crypto.CryptoCompressionFlags.Data | Security.Cryptography.Crypto.CryptoCompressionFlags.IV, o != null ? o.Key : null);
                                        }
                                        data = account._Password.Data.ToArray(crypto);
                                    }
                                    catch (Exception e)
                                    {
                                        Util.Logging.Log(e);
                                        data = null;
                                    }

                                    if (data != null)
                                    {
                                        writer.Write((ushort)data.Length);
                                        writer.Write(data);
                                        writer.Write(account._Password.UID);
                                    }
                                    else
                                    {
                                        writer.Write((ushort)0);
                                    }
                                }

                                if (booleans[9])
                                    writer.Write(account._BackgroundImage);

                                if (booleans[10])
                                {
                                    var im = account._Image;
                                    writer.Write(im.Path);
                                    writer.Write((byte)im.Placement);
                                }

                                if (booleans[12])
                                    writer.Write(account._ScreenshotsLocation);

                                if (booleans[13])
                                {
                                    var pages = account._Pages;
                                    writer.Write((byte)pages.Length);
                                    for (var j = 0; j < pages.Length; j++)
                                    {
                                        writer.Write(pages[j].Page);
                                        writer.Write(pages[j].SortKey);
                                    }
                                }

                                if (booleans[14])
                                {
                                    writer.Write((byte)account._TotpKey.Length);
                                    writer.Write(account._TotpKey);
                                }

                                //if (booleans[15])
                                //{
                                //    var ad = account._ApiData;

                                //    var booleansApi = new bool[]
                                //    {
                                //        ad.DailyPoints != null,
                                //        ad.Played != null
                                //    };

                                //    b = CompressBooleans(booleansApi);

                                //    writer.Write((byte)b.Length);
                                //    writer.Write(b);

                                //    if (booleansApi[0])
                                //    {
                                //        writer.Write(ad._DailyPoints._LastChange.ToBinary());
                                //        writer.Write((byte)ad._DailyPoints._State);
                                //        writer.Write(ad._DailyPoints._Value);
                                //    }

                                //    if (booleansApi[1])
                                //    {
                                //        writer.Write(ad._Played._LastChange.ToBinary());
                                //        writer.Write((byte)ad._Played._State);
                                //        writer.Write(ad._Played._Value);
                                //    }
                                //}

                                if (booleans[16])
                                {
                                    writer.Write(account._WindowBounds.X);
                                    writer.Write(account._WindowBounds.Y);
                                    writer.Write(account._WindowBounds.Width);
                                    writer.Write(account._WindowBounds.Height);
                                }

                                //if (booleans[17])
                                //    writer.Write((byte)account._ProcessPriority);

                                //if (booleans[18])
                                //    writer.Write(account._ClientPort);

                                //if (booleans[19])
                                //    writer.Write(account._LastDailyCompletionUtc.ToBinary());

                                if (booleans[20])
                                    writer.Write((byte)account._Mute);

                                if (booleans[21])
                                    writer.Write((byte)account._ScreenshotsFormat);

                                if (booleans[22])
                                    writer.Write((byte)account._NetworkAuthorizationState);

                                if (booleans[23])
                                    writer.Write((byte)account._ProcessPriority);

                                if (booleans[24])
                                    writer.Write(account._ProcessAffinity);

                                if (booleans[25])
                                {
                                    var notes = account._Notes;

                                    lock (notes)
                                    {
                                        WriteVariableLength(writer, notes.Count);

                                        foreach (var n in notes)
                                        {
                                            writer.Write((ushort)n.SID);
                                            writer.Write(n.Expires.ToBinary());
                                            writer.Write(n.Notify);
                                        }
                                    }
                                }

                                if (booleans[26])
                                    writer.Write(account._ColorKey.ToArgb());

                                if (booleans[27])
                                {
                                    var v = account._IconType;
                                    writer.Write((byte)v);
                                    if (v == IconType.File)
                                    {
                                        var icon = account._Icon;
                                        if (icon == null)
                                            icon = "";
                                        writer.Write(icon);
                                    }
                                }

                                if (booleans[28])
                                    writer.Write(account._SortKey);





                                if (account._Type == AccountType.GuildWars1)
                                {
                                    var a = (Gw1Account)account;

                                    booleans = new bool[]
                                    {
                                        //0
                                        a._DatFile != null,
                                        !string.IsNullOrEmpty(a._CharacterName),
                                        //2
                                    };

                                    b = CompressBooleans(booleans);

                                    writer.Write((byte)b.Length);
                                    writer.Write(b);

                                    if (booleans[0])
                                        writer.Write(a._DatFile.UID);
                                    if (booleans[1])
                                        writer.Write(a._CharacterName);
                                }
                                else
                                {
                                    var a = (Gw2Account)account;

                                    booleans = new bool[]
                                    {
                                        //0
                                        a._DatFile != null,
                                        a._GfxFile != null,
                                        a._AutomaticRememberedLogin,
                                        !string.IsNullOrEmpty(a._ApiKey),
                                        a._ApiData != null,
                                        a._ClientPort != 0,
                                        a._LastDailyCompletionUtc.Ticks != 1,
                                        !string.IsNullOrEmpty(a._MumbleLinkName),
                                        //8
                                    };

                                    b = CompressBooleans(booleans);

                                    writer.Write((byte)b.Length);
                                    writer.Write(b);

                                    if (booleans[0])
                                        writer.Write(a._DatFile.UID);

                                    if (booleans[1])
                                        writer.Write(a._GfxFile.UID);

                                    if (booleans[3])
                                        writer.Write(a._ApiKey);

                                    if (booleans[4])
                                    {
                                        var ad = a._ApiData;

                                        var booleansApi = new bool[]
                                        {
                                            ad.DailyPoints != null,
                                            ad.Played != null
                                        };

                                        b = CompressBooleans(booleansApi);

                                        writer.Write((byte)b.Length);
                                        writer.Write(b);

                                        if (booleansApi[0])
                                        {
                                            writer.Write(ad._DailyPoints._LastChange.ToBinary());
                                            writer.Write((byte)ad._DailyPoints._State);
                                            writer.Write(ad._DailyPoints._Value);
                                        }

                                        if (booleansApi[1])
                                        {
                                            writer.Write(ad._Played._LastChange.ToBinary());
                                            writer.Write((byte)ad._Played._State);
                                            writer.Write(ad._Played._Value);
                                        }
                                    }

                                    if (booleans[5])
                                        writer.Write(a._ClientPort);

                                    if (booleans[6])
                                        writer.Write(a._LastDailyCompletionUtc.ToBinary());

                                    if (booleans[7])
                                        writer.Write(a._MumbleLinkName);
                                }
                            }
                        }
                        finally
                        {
                            if (crypto != null)
                                crypto.Dispose();
                        }
                    }

                    lock (_HiddenUserAccounts)
                    {
                        var count = _HiddenUserAccounts.Count;
                        var items = new string[count];
                        int i = 0;

                        foreach (var key in _HiddenUserAccounts.Keys)
                        {
                            if (i == count)
                                break;
                            var o = _HiddenUserAccounts[key];
                            if (o.HasValue && o.Value)
                            {
                                items[i++] = key;
                            }
                        }

                        count = i;

                        writer.Write((ushort)count);

                        for (i = 0; i < count; i++)
                        {
                            var item = items[i];
                            writer.Write(item);
                        }
                    }

                    //v4
                    lock (_HiddenDailyCategories)
                    {
                        var count = _HiddenDailyCategories.Count;
                        var items = new byte[count];
                        int i = 0;

                        foreach (var key in _HiddenDailyCategories.Keys)
                        {
                            if (i == count)
                                break;
                            var o = _HiddenDailyCategories[key];
                            if (o.HasValue && o.Value)
                            {
                                items[i++] = key;
                            }
                        }

                        count = i;

                        writer.Write((byte)count);

                        for (i = 0; i < count; i++)
                        {
                            var item = items[i];
                            writer.Write(item);
                        }
                    }







                    writer.Write((ushort)crc.CRC);
                }

                _settingsFile.Save();
            }
            catch (Exception e)
            {
#if DEBUG
                System.Diagnostics.Debugger.Break();
#endif
                Util.Logging.Log(e);
                return e;
            }

            return null;
        }

        private static ushort Load(string path)
        {
            ushort version;
            IO.CrcStream crc;

            using (var reader = new BinaryReader(crc = new IO.CrcStream(new BufferedStream(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)), false)))
            {
                byte[] header = reader.ReadBytes(HEADER.Length);
                if (!Compare(HEADER, header))
                    throw new IOException("Invalid header");
                version = reader.ReadUInt16();

                lock (_WindowBounds)
                {
                    _WindowBounds.Clear();

                    var count = reader.ReadByte();
                    for (int i = 0; i < count; i++)
                    {
                        Type t = GetWindow(reader.ReadByte());
                        Rectangle r = new Rectangle(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());

                        if (t != null && !r.IsEmpty)
                        {
                            SettingValue<Rectangle> item = new SettingValue<Rectangle>();
                            item.SetValue(r);
                            _WindowBounds.Add(t, item);
                        }
                    }
                }

                byte[] b = reader.ReadBytes(reader.ReadByte());
                bool[] booleans = ExpandBooleans(b);

                if (version >= 10)
                {
                    if (booleans[0])
                        _Sorting.SetValue(new SortingOptions(reader.ReadBytes(SortingOptions.ARRAY_SIZE)));
                    else
                        _Sorting.Clear();

                    if (booleans[1])
                    {
                        var scope = (EncryptionScope)reader.ReadByte();
                        var key = reader.ReadBytes(reader.ReadByte());

                        if (key.Length == 0)
                            key = null;

                        _Encryption.SetValue(new EncryptionOptions(scope, key));
                    }
                    else
                        _Encryption.Clear();
                }
                else
                {
                    var sorting = SortMode.None;
                    var descending = false;

                    if (booleans[0] || booleans[1])
                    {
                        if (booleans[0])
                            sorting = (SortMode)reader.ReadByte();

                        if (booleans[1])
                            descending = (SortOrder)reader.ReadByte() == SortOrder.Descending;

                        _Sorting.SetValue(new SortingOptions(sorting, descending, GroupMode.None, false));
                    }
                    else
                        _Sorting.Clear();
                }

                if (booleans[2])
                    _StoreCredentials.SetValue(booleans[13]);
                else
                    _StoreCredentials.Clear();

                if (booleans[3])
                    _ShowTray.SetValue(booleans[14]);
                else
                    _ShowTray.Clear();

                if (booleans[4])
                    _MinimizeToTray.SetValue(booleans[15]);
                else
                    _MinimizeToTray.Clear();

                if (booleans[5])
                    _BringToFrontOnExit.SetValue(booleans[16]);
                else
                    _BringToFrontOnExit.Clear();

                if (booleans[6])
                    _DeleteCacheOnLaunch.SetValue(booleans[17]);
                else
                    _DeleteCacheOnLaunch.Clear();

                if (booleans[7])
                    _GuildWars2._Path.SetValue(reader.ReadString());
                else
                    _GuildWars2._Path.Clear();

                if (booleans[8])
                    _GuildWars2._Arguments.SetValue(reader.ReadString());
                else
                    _GuildWars2._Arguments.Clear();

                if (booleans[9])
                    _LastKnownBuild.SetValue(reader.ReadInt32());
                else
                    _LastKnownBuild.Clear();

                if (booleans[10])
                {
                    Font font = null;
                    try
                    {
                        font = new FontConverter().ConvertFromString(reader.ReadString()) as Font;
                    }
                    catch (Exception ex)
                    {
                        Util.Logging.Log(ex);
                    }
                    if (font != null)
                        _FontName.SetValue(font);
                    else
                        _FontName.Clear();
                }
                else
                    _FontName.Clear();

                if (booleans[11])
                {
                    Font font = null;
                    try
                    {
                        font = new FontConverter().ConvertFromString(reader.ReadString()) as Font;
                    }
                    catch (Exception ex)
                    {
                        Util.Logging.Log(ex);
                    }
                    if (font != null)
                        _FontStatus.SetValue(font);
                    else
                        _FontStatus.Clear();
                }
                else
                    _FontStatus.Clear();

                if (booleans[12])
                    _StyleShowAccount.SetValue(booleans[18]);
                else
                    _StyleShowAccount.Clear();

                if (version >= 2)
                {
                    if (booleans[19])
                        _CheckForNewBuilds.SetValue(booleans[32]);
                    else
                        _CheckForNewBuilds.Clear();

                    if (booleans[20])
                    {
                        _LastProgramVersion.SetValue(new LastCheckedVersion(DateTime.FromBinary(reader.ReadInt64()), reader.ReadUInt16()));
                    }
                    else
                    {
                        _LastProgramVersion.Clear();
                    }

                    if (booleans[21])
                        _AutoUpdateInterval.SetValue(reader.ReadUInt16());
                    else
                        _AutoUpdateInterval.Clear();

                    if (booleans[22])
                        _AutoUpdate.SetValue(booleans[33]);
                    else
                        _AutoUpdate.Clear();

                    if (booleans[23])
                        _BackgroundPatchingEnabled.SetValue(booleans[34]);
                    else
                        _BackgroundPatchingEnabled.Clear();

                    if (booleans[24])
                        _BackgroundPatchingLang.SetValue(reader.ReadByte());
                    else
                        _BackgroundPatchingLang.Clear();

                    if (booleans[25])
                        _BackgroundPatchingNotifications.SetValue(new ScreenAttachment(reader.ReadByte(), (ScreenAnchor)reader.ReadByte()));
                    else
                        _BackgroundPatchingNotifications.Clear();

                    if (booleans[26])
                        _BackgroundPatchingMaximumThreads.SetValue(reader.ReadByte());
                    else
                        _BackgroundPatchingMaximumThreads.Clear();

                    if (booleans[27])
                        _PatchingSpeedLimit.SetValue(reader.ReadInt32());
                    else
                        _PatchingSpeedLimit.Clear();

                    if (version >= 10)
                    {
                        if (booleans[28])
                            _PatchingOptions.SetValue((PatchingFlags)reader.ReadByte());
                        else
                            _PatchingOptions.Clear();
                    }
                    else
                    {
                        if (booleans[28] && booleans[36])
                            _PatchingOptions.SetValue(PatchingFlags.UseHttps);
                        else
                            _PatchingOptions.Clear();
                    }

                    if (booleans[29])
                    {
                        RunAfter[] ras;

                        if (version >= 10)
                        {
                            ras = new RunAfter[ReadVariableLength(reader)];

                            for (var i = 0; i < ras.Length; i++)
                            {
                                ras[i] = new RunAfter(reader.ReadString(), reader.ReadString(), reader.ReadString(), (RunAfter.RunAfterOptions)reader.ReadByte());
                            }
                        }
                        else
                        {
                            ras = new RunAfter[]
                            {
                                new RunAfter(null, null, reader.ReadString(), RunAfter.RunAfterOptions.Enabled),
                            };
                        }

                        _GuildWars2._RunAfter.SetValue(ras);
                    }
                    else
                        _GuildWars2._RunAfter.Clear();

                    if (booleans[30])
                        _GuildWars2._Volume.SetValue(reader.ReadByte() / 255f);
                    else
                        _GuildWars2._Volume.Clear();

                    if (booleans[31])
                        _LocalAssetServerEnabled.SetValue(booleans[35]);
                    else
                        _LocalAssetServerEnabled.Clear();
                }

                if (version >= 10)
                {
                    if (booleans[36])
                        _PatchingPort.SetValue(reader.ReadUInt16());
                    else
                        _PatchingPort.Clear();
                }

                if (version >= 3)
                {
                    if (booleans[37])
                        _WindowCaption.SetValue(reader.ReadString());
                    else
                        _WindowCaption.Clear();

                    if (booleans[38])
                        _PreventTaskbarGrouping.SetValue(booleans[39]);
                    else
                        _PreventTaskbarGrouping.Clear();
                }

                if (version >= 4)
                {
                    if (booleans[40])
                        _GuildWars2._AutomaticRememberedLogin.SetValue(booleans[61]);
                    else
                        _GuildWars2._AutomaticRememberedLogin.Clear();

                    if (booleans[41])
                        _GuildWars2._Mute.SetValue((MuteOptions)reader.ReadByte());
                    else
                        _GuildWars2._Mute.Clear();

                    if (booleans[42])
                        _GuildWars2._ClientPort.SetValue(reader.ReadUInt16());
                    else
                        _GuildWars2._ClientPort.Clear();

                    if (booleans[43])
                        _GuildWars2._ScreenshotsFormat.SetValue((ScreenshotFormat)reader.ReadByte());
                    else
                        _GuildWars2._ScreenshotsFormat.Clear();

                    if (booleans[44])
                        _GuildWars2._ScreenshotsLocation.SetValue(reader.ReadString());
                    else
                        _GuildWars2._ScreenshotsLocation.Clear();

                    if (booleans[45])
                        _TopMost.SetValue(booleans[62]);
                    else
                        _TopMost.Clear();

                    if (booleans[46])
                        _GuildWars2._VirtualUserPath.SetValue(reader.ReadString());
                    else
                        _GuildWars2._VirtualUserPath.Clear();

                    if (booleans[47])
                        _GuildWars2._VirtualUserPath.SetCommit(reader.ReadString());
                    else
                        _GuildWars2._VirtualUserPath.ClearCommit();

                    if (booleans[48])
                        _ActionActiveLClick.SetValue((ButtonAction)reader.ReadByte());
                    else
                        _ActionActiveLClick.Clear();

                    if (booleans[49])
                        _ActionActiveLPress.SetValue((ButtonAction)reader.ReadByte());
                    else
                        _ActionActiveLPress.Clear();

                    if (booleans[50])
                        _ShowDailies.SetValue((DailiesMode)reader.ReadByte());
                    else
                        _ShowDailies.Clear();

                    if (booleans[51])
                        _BackgroundPatchingProgress.SetValue(new Rectangle(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32()));
                    else
                        _BackgroundPatchingProgress.Clear();

                    if (booleans[52])
                        _NetworkAuthorization.SetValue((NetworkAuthorizationFlags)reader.ReadByte());
                    else
                        _NetworkAuthorization.Clear();

                    if (booleans[53])
                        _ScreenshotNaming.SetValue(reader.ReadString());
                    else
                        _ScreenshotNaming.Clear();

                    if (booleans[54])
                    {
                        _ScreenshotConversion.SetValue(new ScreenshotConversionOptions()
                        {
                            Format = (ScreenshotConversionOptions.ImageFormat)reader.ReadByte(),
                            raw = reader.ReadByte(),
                        });
                    }
                    else
                        _ScreenshotConversion.Clear();

                    if (booleans[55])
                        _DeleteCrashLogsOnLaunch.SetValue(booleans[63]);
                    else
                        _DeleteCrashLogsOnLaunch.Clear();

                    if (booleans[56])
                        _GuildWars2._ProcessPriority.SetValue((ProcessPriorityClass)reader.ReadByte());
                    else
                        _GuildWars2._ProcessPriority.Clear();

                    if (booleans[57])
                        _GuildWars2._ProcessAffinity.SetValue(reader.ReadInt64());
                    else
                        _GuildWars2._ProcessAffinity.Clear();

                    if (booleans[58])
                        _GuildWars2._PrioritizeCoherentUI.SetValue(booleans[64]);
                    else
                        _GuildWars2._PrioritizeCoherentUI.Clear();

                    if (booleans[59])
                        _NotesNotifications.SetValue(new NotificationScreenAttachment(reader.ReadByte(), (ScreenAnchor)reader.ReadByte(), reader.ReadBoolean()));
                    else
                        _NotesNotifications.Clear();

                    if (booleans[60])
                        _MaxPatchConnections.SetValue(reader.ReadByte());
                    else
                        _MaxPatchConnections.Clear();
                }

                if (version >= 5)
                {
                    if (booleans[65])
                        _ActionInactiveLClick.SetValue((ButtonAction)reader.ReadByte());
                    else
                        _ActionInactiveLClick.Clear();

                    if (booleans[66])
                        _StyleShowColor.SetValue(booleans[82]);
                    else
                        _StyleShowColor.Clear();

                    if (booleans[67])
                        _StyleHighlightFocused.SetValue(booleans[83]);
                    else
                        _StyleHighlightFocused.Clear();

                    if (booleans[68])
                        _WindowIcon.SetValue(booleans[84]);
                    else
                        _WindowIcon.Clear();

                    if (booleans[69])
                        _AccountBarEnabled.SetValue(booleans[85]);
                    else
                        _AccountBarEnabled.Clear();

                    if (booleans[70])
                        _AccountBarStyle.SetValue((AccountBarStyles)reader.ReadByte());
                    else
                        _AccountBarStyle.Clear();

                    if (booleans[71])
                        _AccountBarOptions.SetValue((AccountBarOptions)reader.ReadByte());
                    else
                        _AccountBarOptions.Clear();

                    if (version >= 10)
                    {
                        if (booleans[72])
                            _AccountBarSorting.SetValue(new SortingOptions(reader.ReadBytes(SortingOptions.ARRAY_SIZE)));
                        else
                            _AccountBarSorting.Clear();

                        if (booleans[73])
                            _ScreenshotNotifications.SetValue(new ScreenAttachment(reader.ReadByte(), (ScreenAnchor)reader.ReadByte()));
                        else
                            _ScreenshotNotifications.Clear();
                    }
                    else
                    {
                        if (booleans[72] || booleans[73])
                        {
                            var sorting = SortMode.None;
                            var descending = false;

                            if (booleans[72])
                                sorting = (SortMode)reader.ReadByte();
                            if (booleans[73])
                                descending = (SortOrder)reader.ReadByte() == SortOrder.Descending;

                            _AccountBarSorting.SetValue(new SortingOptions(sorting, descending, GroupMode.None, false));
                        }
                        else
                            _AccountBarSorting.Clear();

                        if (booleans[71])
                        {
                            var flagGroupBy = (AccountBarOptions)2; //AccountBarOptions.GroupByActive
                            if (_AccountBarOptions.Value.HasFlag(flagGroupBy))
                            {
                                _AccountBarOptions.SetValue(_AccountBarOptions.Value & ~flagGroupBy);
                                var v = _AccountBarSorting.Value;
                                _AccountBarSorting.SetValue(new SortingOptions(v.Sorting, v.Grouping.Mode | GroupMode.Active, v.Grouping.Descending));
                            }
                        }
                    }

                    if (booleans[74])
                        _AccountBarDocked.SetValue((ScreenAnchor)reader.ReadByte());
                    else
                        _AccountBarDocked.Clear();

                    if (booleans[75])
                        _UseDefaultIconForShortcuts.SetValue(booleans[86]);
                    else
                        _UseDefaultIconForShortcuts.Clear();

                    if (booleans[76])
                        _LimitActiveAccounts.SetValue(reader.ReadByte());
                    else
                        _LimitActiveAccounts.Clear();

                    if (booleans[77])
                        _DelayLaunchUntilLoaded.SetValue(booleans[87]);
                    else
                        _DelayLaunchUntilLoaded.Clear();

                    if (booleans[78])
                        _DelayLaunchSeconds.SetValue(reader.ReadByte());
                    else
                        _DelayLaunchSeconds.Clear();

                    if (booleans[79])
                    {
                        if (version >= 7)
                            _GuildWars2._LocalizeAccountExecution.SetValue((LocalizeAccountExecutionOptions)reader.ReadByte());
                        else
                            _GuildWars2._LocalizeAccountExecution.SetValue(booleans[88] ? LocalizeAccountExecutionOptions.Enabled : LocalizeAccountExecutionOptions.None);
                    }
                    else
                        _GuildWars2._LocalizeAccountExecution.Clear();

                    if (booleans[80])
                    {
                        if (version >= 7)
                            _GuildWars2._LocalizeAccountExecution.SetCommit((LocalizeAccountExecutionOptions)reader.ReadByte());
                        else
                            _GuildWars2._LocalizeAccountExecution.SetValue(booleans[89] ? LocalizeAccountExecutionOptions.Enabled : LocalizeAccountExecutionOptions.None);
                    }
                    else
                        _GuildWars2._LocalizeAccountExecution.ClearCommit();

                    if (booleans[81])
                    {
                        var v = new LauncherPoints()
                        {
                            EmptyArea = new Point<ushort>(reader.ReadUInt16(), reader.ReadUInt16()),
                            PlayButton = new Point<ushort>(reader.ReadUInt16(), reader.ReadUInt16()),
                        };
                        _GuildWars2._LauncherAutologinPoints.SetValue(v);
                    }
                    else
                        _GuildWars2._LauncherAutologinPoints.Clear();
                }

                if (version >= 7)
                {
                    if (booleans[88])
                        _GuildWars2._DatUpdaterEnabled.SetValue(booleans[92]);
                    else
                        _GuildWars2._DatUpdaterEnabled.Clear();

                    if (booleans[89])
                        _GuildWars2._UseCustomGw2Cache.SetValue(booleans[93]);
                    else
                        _GuildWars2._UseCustomGw2Cache.Clear();

                    if (booleans[90])
                        _ShowKillAllAccounts.SetValue(booleans[94]);
                    else
                        _ShowKillAllAccounts.Clear();

                    if (booleans[91])
                        _GuildWars2._PreventDefaultCoherentUI.SetValue(booleans[95]);
                    else
                        _GuildWars2._PreventDefaultCoherentUI.Clear();
                }

                if (version >= 8)
                {
                    if (booleans[96])
                        _RepaintInitialWindow.SetValue(booleans[97]);
                    else
                        _RepaintInitialWindow.Clear();
                }

                if (version >= 10)
                {
                    if (booleans[98])
                        _GuildWars2._MumbleLinkName.SetValue(reader.ReadString());
                    else
                        _GuildWars2._MumbleLinkName.Clear();

                    if (booleans[99])
                        _GuildWars2._ProfileMode.SetValue((ProfileMode)reader.ReadByte());
                    else
                        _GuildWars2._ProfileMode.Clear();

                    if (booleans[100])
                        _GuildWars2._ProfileOptions.SetValue((ProfileModeOptions)reader.ReadByte());
                    else
                        _GuildWars2._ProfileOptions.Clear();

                    if (booleans[101])
                    {
                        Font font = null;
                        try
                        {
                            font = new FontConverter().ConvertFromString(reader.ReadString()) as Font;
                        }
                        catch (Exception ex)
                        {
                            Util.Logging.Log(ex);
                        }
                        if (font != null)
                            _FontUser.SetValue(font);
                        else
                            _FontUser.Clear();

                        _FontUser.SetValue(font);
                    }
                    else
                        _FontUser.Clear();

                    if (booleans[102])
                        _StyleColumns.SetValue(reader.ReadByte());
                    else
                        _StyleColumns.Clear();

                    if (booleans[103])
                        _StyleShowIcon.SetValue(booleans[104]);
                    else
                        _StyleShowIcon.Clear();

                    if (booleans[105])
                        _StyleBackgroundImage.SetValue(reader.ReadString());
                    else
                        _StyleBackgroundImage.Clear();

                    if (booleans[106])
                    {
                        var colors = new AccountGridButtonColors();
                        var count = reader.ReadByte();

                        for (var i = 0; i < count; i++)
                        {
                            colors.Values[i] = Color.FromArgb(reader.ReadInt32());
                        }

                        _StyleColors.SetValue(colors);
                    }
                    else
                        _StyleColors.Clear();

                    if (booleans[107])
                        _GuildWars1._Path.SetValue(reader.ReadString());
                    else
                        _GuildWars1._Path.Clear();

                    if (booleans[108])
                        _GuildWars1._Arguments.SetValue(reader.ReadString());
                    else
                        _GuildWars1._Arguments.Clear();

                    if (booleans[109])
                        _GuildWars1._Mute.SetValue((MuteOptions)reader.ReadByte());
                    else
                        _GuildWars1._Mute.Clear();

                    if (booleans[110])
                        _GuildWars1._ProcessAffinity.SetValue(reader.ReadInt64());
                    else
                        _GuildWars1._ProcessAffinity.Clear();

                    if (booleans[111])
                        _GuildWars1._ProcessPriority.SetValue((ProcessPriorityClass)reader.ReadByte());
                    else
                        _GuildWars1._ProcessPriority.Clear();

                    if (booleans[112])
                    {
                        var ras = new RunAfter[ReadVariableLength(reader)];

                        for (var i = 0; i < ras.Length; i++)
                        {
                            ras[i] = new RunAfter(reader.ReadString(), reader.ReadString(), reader.ReadString(), (RunAfter.RunAfterOptions)reader.ReadByte());
                        }

                        _GuildWars1._RunAfter.SetValue(ras);
                    }
                    else
                        _GuildWars1._RunAfter.Clear();

                    if (booleans[113])
                        _GuildWars1._ScreenshotsFormat.SetValue((ScreenshotFormat)reader.ReadByte());
                    else
                        _GuildWars1._ScreenshotsFormat.Clear();

                    if (booleans[114])
                        _GuildWars1._ScreenshotsLocation.SetValue(reader.ReadString());
                    else
                        _GuildWars1._ScreenshotsLocation.Clear();

                    if (booleans[115])
                        _GuildWars1._Volume.SetValue(reader.ReadByte() / 255f);
                    else
                        _GuildWars1._Volume.Clear();

                    if (booleans[116])
                    {
                        _WindowTemplates.Clear();

                        var wt = new WindowTemplate[ReadVariableLength(reader)];
                        for (var i = 0; i < wt.Length; i++)
                        {
                            var screens = new WindowTemplate.Screen[ReadVariableLength(reader)];
                            for (var j = 0; j < screens.Length;j++)
                            {
                                var bounds = ReadRectangle(reader);
                                var rects = new Rectangle[ReadVariableLength(reader)];
                                for (var k = 0; k < rects.Length;k++)
                                {
                                    rects[k] = ReadRectangle(reader);
                                }
                                screens[j] = new WindowTemplate.Screen(bounds,rects);
                            }
                            wt[i] = new WindowTemplate(screens);
                        }

                        _WindowTemplates._list.AddRange(wt);
                    }
                    else
                        _WindowTemplates.Clear();

                    if (booleans[117])
                        _LaunchBehindOtherAccounts.SetValue(booleans[118]);
                    else
                        _LaunchBehindOtherAccounts.Clear();

                    if (booleans[119])
                        _LaunchLimiter.SetValue(new LaunchLimiterOptions(reader.ReadByte(), reader.ReadByte(), reader.ReadByte()));
                    else
                        _LaunchLimiter.Clear();

                    if (booleans[120])
                        _ShowLaunchAllAccounts.SetValue(booleans[121]);
                    else
                        _ShowLaunchAllAccounts.Clear();

                    if (booleans[122])
                        _LaunchTimeout.SetValue(reader.ReadByte());
                    else
                        _LaunchTimeout.Clear();

                    if (booleans[123])
                        _SelectedPage.SetValue(reader.ReadByte());
                    else
                        _SelectedPage.Clear();

                    if (booleans[124])
                        _JumpList.SetValue((JumpListOptions)reader.ReadByte());
                    else
                        _JumpList.Clear();

                    if (booleans[125])
                        _PreventTaskbarMinimize.SetValue(booleans[126]);
                    else
                        _PreventTaskbarMinimize.Clear();

                    if (booleans[127])
                        _GuildWars2._PreventRelaunching.SetValue((RelaunchOptions)reader.ReadByte());
                    else
                        _GuildWars2._PreventRelaunching.Clear();

                    if (booleans[128])
                        _ActionActiveMClick.SetValue((ButtonAction)reader.ReadByte());
                    else
                        _ActionActiveMClick.Clear();

                    if (booleans[129])
                        _ActionInactiveMClick.SetValue((ButtonAction)reader.ReadByte());
                    else
                        _ActionInactiveMClick.Clear();

                    if (booleans[130])
                        _AuthenticatorPastingEnabled.SetValue(booleans[131]);
                    else
                        _AuthenticatorPastingEnabled.Clear();
                    
                    if (booleans[132])
                        _ProcessPriority.SetValue((ProcessPriorityClass)reader.ReadByte());
                    else
                        _ProcessPriority.Clear();

                    if (booleans[133])
                        _PublicIPAddress.SetValue(reader.ReadBytes(reader.ReadByte()));
                    else
                        _PublicIPAddress.Clear();
                }

                _datUID = 0;

                lock (_DatFiles)
                {
                    _DatFiles.Clear();

                    var count = reader.ReadUInt16();
                    for (int i = 0; i < count; i++)
                    {
                        var s = new DatFile();
                        s._UID = reader.ReadUInt16();
                        s._Path = reader.ReadString();

                        //s._IsInitialized = reader.ReadBoolean(); //v6
                        s._flags = (BaseFile.DataFlags)reader.ReadByte();

                        _DatFiles.Add(s.UID, new SettingValue<IDatFile>(s));

                        if (_datUID < s.UID)
                            _datUID = s.UID;
                    }
                }

                if (version >= 4)
                {
                    _gfxUID = 0;

                    lock (_GfxFiles)
                    {
                        _GfxFiles.Clear();

                        var count = reader.ReadUInt16();
                        for (int i = 0; i < count; i++)
                        {
                            var s = new GfxFile();
                            s._UID = reader.ReadUInt16();
                            s._Path = reader.ReadString();
                            //s._IsInitialized = reader.ReadBoolean(); //v6
                            s._flags = (BaseFile.DataFlags)reader.ReadByte();

                            _GfxFiles.Add(s.UID, new SettingValue<IGfxFile>(s));

                            if (_gfxUID < s.UID)
                                _gfxUID = s.UID;
                        }
                    }
                }
                else
                {
                    _gfxUID = 0;

                    lock (_GfxFiles)
                    {
                        _GfxFiles.Clear();
                    }
                }

                if (version >= 10)
                {
                    _gwdatUID = 0;

                    lock (_GwDatFiles)
                    {
                        _GwDatFiles.Clear();

                        var count = reader.ReadUInt16();
                        for (int i = 0; i < count; i++)
                        {
                            var s = new GwDatFile();
                            s._UID = reader.ReadUInt16();
                            s._Path = reader.ReadString();

                            //s._IsInitialized = reader.ReadBoolean(); //v6
                            s._flags = (BaseFile.DataFlags)reader.ReadByte();

                            _GwDatFiles.Add(s.UID, new SettingValue<IGwDatFile>(s));

                            if (_gwdatUID < s.UID)
                                _gwdatUID = s.UID;
                        }
                    }
                }
                else
                {
                    _gwdatUID = 0;

                    lock (_GwDatFiles)
                    {
                        _GwDatFiles.Clear();
                    }
                }

                _accountUID = 0;
                var sids = new HashSet<ushort>();

                lock (_Accounts)
                {
                    _Accounts.Clear();

                    var count = reader.ReadUInt16();
                    var accounts = new Account[count];
                    Security.Cryptography.Crypto crypto = null;

                    try
                    {
                        for (int i = 0; i < count; i++)
                        {
                            #region Load account

                            Settings.AccountType type;
                            Account account;

                            if (version >= 10)
                            {
                                type = (AccountType)reader.ReadByte();

                                switch (type)
                                {
                                    case AccountType.GuildWars1:
                                        account = new Gw1Account();
                                        break;
                                    case AccountType.GuildWars2:
                                        account = new Gw2Account();
                                        break;
                                    default:
                                        throw new NotSupportedException();
                                }
                            }
                            else
                            {
                                type = AccountType.GuildWars2;
                                account = new Gw2Account();
                            }

                            accounts[i] = account;

                            account._UID = reader.ReadUInt16();
                            account._Name = reader.ReadString();
                            account._WindowsAccount = reader.ReadString();
                            account._CreatedUtc = DateTime.FromBinary(reader.ReadInt64());
                            account._LastUsedUtc = DateTime.FromBinary(reader.ReadInt64());
                            account._TotalUses = reader.ReadUInt16();
                            account._Arguments = reader.ReadString();

                            b = reader.ReadBytes(reader.ReadByte());
                            booleans = ExpandBooleans(b);

                            account._ShowDailyLogin = booleans[0];
                            account._WindowOptions = booleans[1] ? WindowOptions.Windowed : WindowOptions.None;
                            account._RecordLaunches = booleans[2];

                            if (version >= 9)
                            {
                                if (booleans[1])
                                {
                                    account._WindowOptions = (WindowOptions)reader.ReadByte();
                                }
                            }

                            if (version >= 10)
                            {
                                if (booleans[3])
                                    account._AutologinOptions = (AutologinOptions)reader.ReadByte();

                                if (booleans[4])
                                    account._JumpListPinning = (JumpListPinning)reader.ReadByte();
                            }
                            else
                            {
                                if (booleans[4])
                                {
                                    ((Gw2Account)account)._DatFile = (DatFile)_DatFiles[reader.ReadUInt16()].Value;
                                    ((Gw2Account)account)._DatFile.ReferenceCount++;
                                }

                                if (version <= 3)
                                {
                                    account._WindowBounds = new Rectangle(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());

                                    if (booleans[3])
                                    {
                                        account._Email = reader.ReadString();
                                        var s = reader.ReadString();
                                        account._Password = new ProtectedString(Security.Credentials.FromString(ref s));
                                    }
                                }
                                else
                                {
                                    if (booleans[3])
                                        account._AutologinOptions |= AutologinOptions.Login;
                                }
                            }

                            if (version >= 2)
                            {
                                if (booleans[5])
                                    account._Volume = reader.ReadByte();
                                if (booleans[6])
                                {
                                    RunAfter[] ras;

                                    if (version >= 10)
                                    {
                                        ras = new RunAfter[ReadVariableLength(reader)];

                                        for (var j = 0; j < ras.Length; j++)
                                        {
                                            ras[j] = new RunAfter(reader.ReadString(), reader.ReadString(), reader.ReadString(), (RunAfter.RunAfterOptions)reader.ReadByte());
                                        }
                                    }
                                    else
                                    {
                                        ras = new RunAfter[]
                                        {
                                            new RunAfter(null, null, reader.ReadString(), RunAfter.RunAfterOptions.Enabled),
                                        };
                                    }

                                    account._RunAfter = ras;
                                }
                            }

                            if (version >= 4)
                            {
                                if (booleans[7])
                                    account._Email = reader.ReadString();
                                if (booleans[8])
                                {
                                    ushort length = reader.ReadUInt16();
                                    if (length > 0)
                                    {
                                        byte[] data = reader.ReadBytes(length);
                                        ushort puid;

                                        if (crypto == null)
                                        {
                                            var o = _Encryption.Value;
                                            crypto = new Security.Cryptography.Crypto(o != null ? o.Scope : EncryptionScope.CurrentUser, Security.Cryptography.Crypto.CryptoCompressionFlags.Data | Security.Cryptography.Crypto.CryptoCompressionFlags.IV, o != null ? o.Key : null);
                                        }

                                        if (version >= 10)
                                        {
                                            puid = reader.ReadUInt16();
                                        }
                                        else
                                        {
                                            puid = 0;

                                            byte[] buffer = null;

                                            try
                                            {
                                                buffer = crypto.Decrypt(EncryptionScope.CurrentUser, data, null, new byte[] { 99, 12, 55, 17, 45, 97, 83, 64, 38 });
                                                data = crypto.Compress(crypto.Encrypt(buffer));
                                            }
                                            catch (Exception e)
                                            {
                                                data = null;
                                                Util.Logging.Log(e);
                                            }
                                            finally
                                            {
                                                if (buffer != null)
                                                    Array.Clear(buffer, 0, buffer.Length);
                                            }
                                        }

                                        if (data != null)
                                        {
                                            try
                                            {
                                                account._Password = new ProtectedString(crypto, data, puid);
                                            }
                                            catch (Exception e)
                                            {
                                                Util.Logging.Log(e);
                                            }
                                        }
                                    }
                                }

                                if (version >= 10)
                                {
                                    if (booleans[9])
                                        account._BackgroundImage = reader.ReadString();

                                    if (booleans[10])
                                        account._Image = new ImageOptions(reader.ReadString(), (ImagePlacement)reader.ReadByte());

                                    account._PendingFiles = booleans[11];
                                }
                                else
                                {
                                    ((Gw2Account)account)._AutomaticRememberedLogin = booleans[9];

                                    if (booleans[10])
                                    {
                                        ((Gw2Account)account)._GfxFile = (GfxFile)_GfxFiles[reader.ReadUInt16()].Value;
                                        ((Gw2Account)account)._GfxFile.ReferenceCount++;
                                    }

                                    //v6:LocalAppData changed
                                    //v10:AppData structure changed
                                    account._PendingFiles = true;
                                }

                                if (booleans[12])
                                    account._ScreenshotsLocation = reader.ReadString();

                                if (version >= 10)
                                {
                                    if (booleans[13])
                                    {
                                        var pages = new PageData[reader.ReadByte()];

                                        for (var j = 0; j < pages.Length; j++)
                                        {
                                            pages[j] = new PageData(reader.ReadByte(), reader.ReadUInt16());
                                        }

                                        if (pages.Length == 0)
                                            pages = null;

                                        account._Pages = pages;
                                    }
                                }
                                else
                                {
                                    if (booleans[13])
                                        ((Gw2Account)account)._ApiKey = reader.ReadString();
                                }

                                if (booleans[14])
                                    account._TotpKey = reader.ReadBytes(reader.ReadByte());

                                if (version >= 10)
                                {
                                    if (booleans[15])
                                    {
                                        //notused
                                    }
                                }
                                else
                                {
                                    if (booleans[15])
                                    {
                                        var ad = new AccountApiData();

                                        var booleansApi = ExpandBooleans(reader.ReadBytes(reader.ReadByte()));

                                        if (booleansApi[0])
                                        {
                                            ad._DailyPoints = new ApiValue<ushort>()
                                            {
                                                _LastChange = DateTime.FromBinary(reader.ReadInt64()),
                                                _State = (ApiCacheState)reader.ReadByte(),
                                                _Value = reader.ReadUInt16(),
                                            };
                                        }

                                        if (booleansApi[1])
                                        {
                                            ad._Played = new ApiValue<int>()
                                            {
                                                _LastChange = DateTime.FromBinary(reader.ReadInt64()),
                                                _State = (ApiCacheState)reader.ReadByte(),
                                                _Value = reader.ReadInt32(),
                                            };
                                        }

                                        ((Gw2Account)account)._ApiData = ad;
                                    }
                                }

                                if (booleans[16])
                                    account._WindowBounds = new Rectangle(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());

                                //if (booleans[17])
                                //    account._ProcessPriority = (ProcessPriorityClass)reader.ReadByte();

                                if (version <= 9)
                                {
                                    if (booleans[18])
                                        ((Gw2Account)account)._ClientPort = reader.ReadUInt16();

                                    if (booleans[19])
                                        ((Gw2Account)account)._LastDailyCompletionUtc = DateTime.FromBinary(reader.ReadInt64());
                                }

                                if (booleans[20])
                                    account._Mute = (MuteOptions)reader.ReadByte();

                                if (booleans[21])
                                    account._ScreenshotsFormat = (ScreenshotFormat)reader.ReadByte();

                                if (booleans[22])
                                    account._NetworkAuthorizationState = (NetworkAuthorizationState)reader.ReadByte();

                                if (booleans[23])
                                    account._ProcessPriority = (ProcessPriorityClass)reader.ReadByte();

                                if (booleans[24])
                                    account._ProcessAffinity = reader.ReadInt64();

                                if (booleans[25])
                                {
                                    var length = ReadVariableLength(reader);

                                    if (length > 0)
                                    {
                                        var notes = new Notes.Note[length];

                                        for (var j = 0; j < length; j++)
                                        {
                                            var sid = reader.ReadUInt16();
                                            var expires = DateTime.FromBinary(reader.ReadInt64());
                                            var notify = reader.ReadBoolean();

                                            sids.Add(sid);

                                            notes[j] = new Notes.Note(expires, sid, notify);
                                        }

                                        account._Notes = new Notes(notes);
                                    }
                                }
                            }

                            if (version >= 5)
                            {
                                if (booleans[26])
                                    account._ColorKey = Color.FromArgb(reader.ReadInt32());

                                if (booleans[27])
                                {
                                    var v = (IconType)reader.ReadByte();
                                    if (v == IconType.File)
                                    {
                                        account._Icon = reader.ReadString();
                                        if (account._Icon.Length == 0)
                                            v = IconType.None;
                                    }
                                    account._IconType = v;
                                }

                                if (booleans[28])
                                    account._SortKey = reader.ReadUInt16();
                                else
                                    account._SortKey = (ushort)(i + 1);

                                if (version <= 9)
                                {
                                    if (booleans[29])
                                        account._AutologinOptions |= AutologinOptions.Play;
                                }
                            }
                            else
                            {
                                account._SortKey = (ushort)(i + 1);
                                account._AutologinOptions = AutologinOptions.None;
                            }

                            if (version >= 10)
                            {
                                if (type == AccountType.GuildWars1)
                                {
                                    var a = (Gw1Account)account;

                                    b = reader.ReadBytes(reader.ReadByte());
                                    booleans = ExpandBooleans(b);

                                    if (booleans[0])
                                    {
                                        a._DatFile = (GwDatFile)_GwDatFiles[reader.ReadUInt16()].Value;
                                        a._DatFile.ReferenceCount++;
                                    }

                                    if (booleans[1])
                                        a._CharacterName = reader.ReadString();
                                }
                                else
                                {
                                    var a = (Gw2Account)account;

                                    b = reader.ReadBytes(reader.ReadByte());
                                    booleans = ExpandBooleans(b);

                                    if (booleans[0])
                                    {
                                        a._DatFile = (DatFile)_DatFiles[reader.ReadUInt16()].Value;
                                        a._DatFile.ReferenceCount++;
                                    }

                                    if (booleans[1])
                                    {
                                        a._GfxFile = (GfxFile)_GfxFiles[reader.ReadUInt16()].Value;
                                        a._GfxFile.ReferenceCount++;
                                    }

                                    a._AutomaticRememberedLogin = booleans[2];

                                    if (booleans[3])
                                        a._ApiKey = reader.ReadString();

                                    if (booleans[4])
                                    {
                                        var ad = new AccountApiData();

                                        var booleansApi = ExpandBooleans(reader.ReadBytes(reader.ReadByte()));

                                        if (booleansApi[0])
                                        {
                                            ad._DailyPoints = new ApiValue<ushort>()
                                            {
                                                _LastChange = DateTime.FromBinary(reader.ReadInt64()),
                                                _State = (ApiCacheState)reader.ReadByte(),
                                                _Value = reader.ReadUInt16(),
                                            };
                                        }

                                        if (booleansApi[1])
                                        {
                                            ad._Played = new ApiValue<int>()
                                            {
                                                _LastChange = DateTime.FromBinary(reader.ReadInt64()),
                                                _State = (ApiCacheState)reader.ReadByte(),
                                                _Value = reader.ReadInt32(),
                                            };
                                        }

                                        a._ApiData = ad;
                                    }

                                    if (booleans[5])
                                        a._ClientPort = reader.ReadUInt16();

                                    if (booleans[6])
                                        a._LastDailyCompletionUtc = DateTime.FromBinary(reader.ReadInt64());

                                    if (booleans[7])
                                        a._MumbleLinkName = reader.ReadString();
                                }
                            }




                            #endregion

                            SettingValue<IAccount> item = new SettingValue<IAccount>();
                            item.SetValue(account);
                            _Accounts.Add(account._UID, item);

                            if (_accountUID < account._UID)
                                _accountUID = account._UID;
                        }
                    }
                    finally
                    {
                        if (crypto != null)
                            crypto.Dispose();
                    }

                    if (version <= 9) //sortKeySum != count * (count + 1) / 2
                    {
                        Array.Sort<Account>(accounts,
                            delegate(Account a1, Account a2)
                            {
                                var c = a1._SortKey.CompareTo(a2._SortKey);
                                if (c == 0)
                                    return a1._UID.CompareTo(a2._UID);
                                return c;
                            });

                        for (var i = 0; i < count; i++)
                        {
                            accounts[i]._SortKey = (ushort)(i + 1);
                        }
                    }
                }

                if (sids.Count > 0)
                {
                    try
                    {
                        using (var notes = new Tools.Notes())
                        {
                            notes.RemoveExcept(sids);
                        }
                    }
                    catch (Exception e)
                    {
                        Util.Logging.Log(e);
                    }
                }

                lock (_HiddenUserAccounts)
                {
                    _HiddenUserAccounts.Clear();

                    var count = reader.ReadUInt16();
                    for (int i = 0; i < count; i++)
                    {
                        _HiddenUserAccounts.Add(reader.ReadString(), new SettingValue<bool>(true));
                    }
                }

                if (version >= 4)
                {
                    lock (_HiddenDailyCategories)
                    {
                        _HiddenDailyCategories.Clear();

                        var count = reader.ReadByte();
                        for (int i = 0; i < count; i++)
                        {
                            _HiddenDailyCategories.Add(reader.ReadByte(), new SettingValue<bool>(true));
                        }
                    }
                }

                #region Upgrade old versions

                if (version <= 1)
                {
                    #region Version 1

                    var args = _GuildWars2._Arguments.Value;
                    if (!string.IsNullOrEmpty(args))
                    {
                        if (args.IndexOf("-l:assetsrv") != -1)
                        {
                            _LocalAssetServerEnabled.SetValue(true);
                            _GuildWars2._Arguments.SetValue(Util.Args.AddOrReplace(args, "l:assetsrv", ""));
                        }
                    }

                    if (_LastKnownBuild.HasValue)
                        _CheckForNewBuilds.SetValue(true);

                    #endregion
                }

                if (version <= 3)
                {
                    #region Version 3

                    try
                    {
                        var pl = Path.Combine(DataPath.AppData, "PL");
                        if (Directory.Exists(pl))
                            Directory.Delete(pl, true);
                    }
                    catch { }

                    string[] keys = new string[] 
                    {
                        "clientport 80",
                        "clientport 443",
                        "autologin",
                        "nosound",
                        "nomusic",
                        "bmp",
                    };

                    var args = _GuildWars2._Arguments.Value;
                    if (!string.IsNullOrWhiteSpace(args))
                    {
                        var changed = false;
                        var ikey = 0;

                        foreach (var key in keys)
                        {
                            if (Util.Args.Contains(args, key))
                            {
                                args = Util.Args.AddOrReplace(args, key, "");
                                changed = true;

                                switch (ikey)
                                {
                                    case 0:
                                        _GuildWars2._ClientPort.SetValue(80);
                                        break;
                                    case 1:
                                        _GuildWars2._ClientPort.SetValue(443);
                                        break;
                                    case 2:
                                        _GuildWars2._AutomaticRememberedLogin.SetValue(true);
                                        break;
                                    case 3:
                                        _GuildWars2._Mute.SetValue(_GuildWars2._Mute.Value | MuteOptions.All);
                                        break;
                                    case 4:
                                        _GuildWars2._Mute.SetValue(_GuildWars2._Mute.Value | MuteOptions.Music);
                                        break;
                                    case 5:
                                        _GuildWars2._ScreenshotsFormat.SetValue(ScreenshotFormat.Bitmap);
                                        break;
                                }
                            }

                            ikey++;
                        }

                        if (changed)
                            _GuildWars2._Arguments.SetValue(args);
                    }

                    var gfxName = "GFXSettings.{0}.xml";
                    var exeName = string.IsNullOrEmpty(_GuildWars2._Path.Value) ? null : Path.GetFileName(_GuildWars2._Path.Value);
                    var gfxs = new Dictionary<ushort, GfxFile>();

                    foreach (var uid in _Accounts.GetKeys())
                    {
                        var account = (Gw2Account)_Accounts[uid].Value;
                        args = account._Arguments;

                        if (!string.IsNullOrWhiteSpace(args))
                        {
                            var changed = false;
                            var ikey = 0;

                            foreach (var key in keys)
                            {
                                if (Util.Args.Contains(args, key))
                                {
                                    args = Util.Args.AddOrReplace(args, key, "");
                                    changed = true;

                                    switch (ikey)
                                    {
                                        case 0:
                                            account._ClientPort = 80;
                                            break;
                                        case 1:
                                            account._ClientPort = 443;
                                            break;
                                        case 2:
                                            account._AutomaticRememberedLogin = true;
                                            break;
                                        case 3:
                                            account._Mute |= MuteOptions.All;
                                            break;
                                        case 4:
                                            account._Mute |= MuteOptions.Music;
                                            break;
                                        case 5:
                                            account._ScreenshotsFormat = ScreenshotFormat.Bitmap;
                                            break;
                                    }
                                }

                                ikey++;
                            }

                            if (changed)
                                account._Arguments = args;
                        }

                        if (exeName != null)
                        {
                            var dat = account._DatFile;
                            if (dat != null && !string.IsNullOrEmpty(dat._Path))
                            {
                                GfxFile gfx;
                                if (!gfxs.TryGetValue(dat._UID, out gfx))
                                {
                                    string gfxpath;
                                    if (dat._Path.EndsWith("." + dat._UID + ".dat"))
                                        gfxpath = string.Format(gfxName, exeName + "." + dat._UID);
                                    else
                                        gfxpath = string.Format(gfxName, exeName);

                                    gfxpath = Path.Combine(Path.GetDirectoryName(dat._Path), gfxpath);

                                    if (File.Exists(gfxpath))
                                    {
                                        gfx = (GfxFile)CreateGfxFile();
                                        gfx._Path = gfxpath;
                                    }

                                    gfxs[account._DatFile._UID] = gfx;
                                }
                                if (gfx != null)
                                    gfx.ReferenceCount++;
                                account._GfxFile = gfx;
                            }
                        }
                    }

                    #endregion
                }

                #endregion

                if (version >= 10)
                {
                    var _crc = crc.CRC;
                    if (_crc != reader.ReadUInt16())
                        throw new IOException("CRC failed");
                }
            }

            return version;
        }

        #region Historical v1 I/O

        //private static void LoadV1(BinaryReader reader)
        //{
        //    lock (_WindowBounds)
        //    {
        //        _WindowBounds.Clear();

        //        var count = reader.ReadByte();
        //        for (int i = 0; i < count; i++)
        //        {
        //            Type t = GetWindow(reader.ReadByte());
        //            Rectangle r = new Rectangle(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());

        //            if (t != null && !r.IsEmpty)
        //            {
        //                SettingValue<Rectangle> item = new SettingValue<Rectangle>();
        //                item.SetValue(r);
        //                _WindowBounds.Add(t, item);
        //            }
        //        }
        //    }

        //    byte[] b = reader.ReadBytes(reader.ReadByte());
        //    bool[] booleans = ExpandBooleans(b);

        //    if (booleans[0])
        //        _SortingMode.SetValue((SortMode)reader.ReadByte());
        //    else
        //        _SortingMode.Clear();

        //    if (booleans[1])
        //        _SortingOrder.SetValue((SortOrder)reader.ReadByte());
        //    else
        //        _SortingOrder.Clear();

        //    if (booleans[2])
        //        _StoreCredentials.SetValue(booleans[13]);
        //    else
        //        _StoreCredentials.Clear();

        //    if (booleans[3])
        //        _ShowTray.SetValue(booleans[14]);
        //    else
        //        _ShowTray.Clear();

        //    if (booleans[4])
        //        _MinimizeToTray.SetValue(booleans[15]);
        //    else
        //        _MinimizeToTray.Clear();

        //    if (booleans[5])
        //        _BringToFrontOnExit.SetValue(booleans[16]);
        //    else
        //        _BringToFrontOnExit.Clear();

        //    if (booleans[6])
        //        _DeleteCacheOnLaunch.SetValue(booleans[17]);
        //    else
        //        _DeleteCacheOnLaunch.Clear();

        //    if (booleans[7])
        //        _GW2Path.SetValue(reader.ReadString());
        //    else
        //        _GW2Path.Clear();

        //    if (booleans[8])
        //        _GW2Arguments.SetValue(reader.ReadString());
        //    else
        //        _GW2Arguments.Clear();

        //    if (booleans[9])
        //        _LastKnownBuild.SetValue(reader.ReadInt32());
        //    else
        //        _LastKnownBuild.Clear();

        //    if (booleans[10])
        //    {
        //        Font font = null;
        //        try
        //        {
        //            font = new FontConverter().ConvertFromString(reader.ReadString()) as Font;
        //        }
        //        catch (Exception ex)
        //        {
        //            Util.Logging.Log(ex);
        //        }
        //        if (font != null)
        //            _FontLarge.Value = font;
        //        else
        //            _FontLarge.Clear();
        //    }
        //    else
        //        _FontLarge.Clear();

        //    if (booleans[11])
        //    {
        //        Font font = null;
        //        try
        //        {
        //            font = new FontConverter().ConvertFromString(reader.ReadString()) as Font;
        //        }
        //        catch (Exception ex)
        //        {
        //            Util.Logging.Log(ex);
        //        }
        //        if (font != null)
        //            _FontSmall.Value = font;
        //        else
        //            _FontSmall.Clear();
        //    }
        //    else
        //        _FontSmall.Clear();

        //    if (booleans[12])
        //        _ShowAccount.Value = booleans[18];
        //    else
        //        _ShowAccount.Clear();

        //    _datUID = 0;

        //    lock (_DatFiles)
        //    {
        //        _DatFiles.Clear();

        //        var count = reader.ReadUInt16();
        //        for (int i = 0; i < count; i++)
        //        {
        //            var s = new DatFile();
        //            s._UID = reader.ReadUInt16();
        //            s._Path = reader.ReadString();
        //            s._IsInitialized = reader.ReadBoolean();

        //            _DatFiles.Add(s.UID, new SettingValue<IDatFile>(s));

        //            if (_datUID < s.UID)
        //                _datUID = s.UID;
        //        }
        //    }

        //    _accountUID = 0;

        //    lock (_Accounts)
        //    {
        //        _Accounts.Clear();

        //        var count = reader.ReadUInt16();
        //        for (int i = 0; i < count; i++)
        //        {
        //            Account account = new Account();
        //            account._UID = reader.ReadUInt16();
        //            account._Name = reader.ReadString();
        //            account._WindowsAccount = reader.ReadString();
        //            account._CreatedUtc = DateTime.FromBinary(reader.ReadInt64());
        //            account._LastUsedUtc = DateTime.FromBinary(reader.ReadInt64());
        //            account._TotalUses = reader.ReadUInt16();
        //            account._Arguments = reader.ReadString();

        //            b = reader.ReadBytes(reader.ReadByte());
        //            booleans = ExpandBooleans(b);

        //            account._ShowDaily = booleans[0];
        //            account._Windowed = booleans[1];
        //            account._RecordLaunches = booleans[2];

        //            if (booleans[4])
        //            {
        //                account._DatFile = (DatFile)_DatFiles[reader.ReadUInt16()].Value;
        //                account._DatFile.ReferenceCount++;
        //            }

        //            account._WindowedBounds = new Rectangle(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());

        //            if (booleans[3])
        //            {
        //                account._AutomaticLoginEmail = reader.ReadString();
        //                account._AutomaticLoginPassword = reader.ReadString();
        //            }

        //            SettingValue<IAccount> item = new SettingValue<IAccount>();
        //            item.SetValue(account);
        //            _Accounts.Add(account._UID, item);

        //            if (_accountUID < account._UID)
        //                _accountUID = account._UID;
        //        }
        //    }


        //    lock (_HiddenUserAccounts)
        //    {
        //        _HiddenUserAccounts.Clear();

        //        var count = reader.ReadUInt16();
        //        for (int i = 0; i < count; i++)
        //        {
        //            _HiddenUserAccounts.Add(reader.ReadString(), new SettingValue<bool>(true));
        //        }
        //    }
        //}

        //private static Exception WriteV1()
        //{
        //    try
        //    {
        //        string path = Path.Combine(DataPath.AppData, FILE_NAME);
        //        using (BinaryWriter writer = new BinaryWriter(new BufferedStream(File.Open(path + ".tmp", FileMode.Create, FileAccess.Write, FileShare.Read))))
        //        {
        //            writer.Write(HEADER);
        //            writer.Write(VERSION);

        //            bool[] booleans;

        //            lock (_WindowBounds)
        //            {
        //                var count = _WindowBounds.Count;
        //                var items = new KeyValuePair<Type, Rectangle>[count];
        //                int i = 0;

        //                foreach (var key in _WindowBounds.Keys)
        //                {
        //                    if (i == count)
        //                        break;
        //                    var o = _WindowBounds[key];
        //                    if (o.HasValue)
        //                    {
        //                        items[i++] = new KeyValuePair<Type, Rectangle>(key, o.Value);
        //                    }
        //                }

        //                count = i;

        //                writer.Write((byte)count);

        //                for (i = 0; i < count; i++)
        //                {
        //                    var item = items[i];

        //                    writer.Write(GetWindowID(item.Key));

        //                    writer.Write(item.Value.X);
        //                    writer.Write(item.Value.Y);
        //                    writer.Write(item.Value.Width);
        //                    writer.Write(item.Value.Height);
        //                }
        //            }

        //            booleans = new bool[]
        //            {
        //                //HasValue
        //                _SortingMode.HasValue && _SortingMode.Value != default(SortMode),
        //                _SortingOrder.HasValue && _SortingOrder.Value != default(SortOrder),
        //                _StoreCredentials.HasValue,
        //                _ShowTray.HasValue,
        //                _MinimizeToTray.HasValue,
        //                _BringToFrontOnExit.HasValue,
        //                _DeleteCacheOnLaunch.HasValue,
        //                _GW2Path.HasValue && !string.IsNullOrWhiteSpace(_GW2Path.Value),
        //                _GW2Arguments.HasValue && !string.IsNullOrWhiteSpace(_GW2Arguments.Value),
        //                _LastKnownBuild.HasValue,
        //                _FontLarge.HasValue,
        //                _FontSmall.HasValue,
        //                _ShowAccount.HasValue,

        //                //Values
        //                _StoreCredentials.Value,
        //                _ShowTray.Value,
        //                _MinimizeToTray.Value,
        //                _BringToFrontOnExit.Value,
        //                _DeleteCacheOnLaunch.Value,
        //                _ShowAccount.Value
        //            };

        //            byte[] b = CompressBooleans(booleans);

        //            writer.Write((byte)b.Length);
        //            writer.Write(b);

        //            if (booleans[0])
        //                writer.Write((byte)_SortingMode.Value);
        //            if (booleans[1])
        //                writer.Write((byte)_SortingOrder.Value);
        //            if (booleans[7])
        //                writer.Write(_GW2Path.Value);
        //            if (booleans[8])
        //                writer.Write(_GW2Arguments.Value);
        //            if (booleans[9])
        //                writer.Write(_LastKnownBuild.Value);
        //            if (booleans[10])
        //            {
        //                try
        //                {
        //                    writer.Write(new FontConverter().ConvertToString(_FontLarge.Value));
        //                }
        //                catch (Exception e)
        //                {
        //                    Util.Logging.Log(e);
        //                    writer.Write("");
        //                }
        //            }
        //            if (booleans[11])
        //            {
        //                try
        //                {
        //                    writer.Write(new FontConverter().ConvertToString(_FontSmall.Value));
        //                }
        //                catch (Exception e)
        //                {
        //                    Util.Logging.Log(e);
        //                    writer.Write("");
        //                }
        //            }

        //            lock (_DatFiles)
        //            {
        //                var count = _DatFiles.Count;
        //                var items = new KeyValuePair<ushort, DatFile>[count];
        //                int i = 0;

        //                foreach (var key in _DatFiles.Keys)
        //                {
        //                    if (i == count)
        //                        break;
        //                    var o = _DatFiles[key];
        //                    if (o.HasValue && ((DatFile)o.Value).ReferenceCount > 0)
        //                    {
        //                        items[i++] = new KeyValuePair<ushort, DatFile>(key, (DatFile)o.Value);
        //                    }
        //                }

        //                count = i;

        //                writer.Write((ushort)count);

        //                for (i = 0; i < count; i++)
        //                {
        //                    var item = items[i];

        //                    writer.Write(item.Value.UID);
        //                    if (string.IsNullOrWhiteSpace(item.Value.Path))
        //                        writer.Write("");
        //                    else
        //                        writer.Write(item.Value.Path);
        //                    writer.Write(item.Value.IsInitialized);
        //                }
        //            }

        //            lock (_Accounts)
        //            {
        //                var count = _Accounts.Count;
        //                var items = new KeyValuePair<ushort, Account>[count];
        //                int i = 0;

        //                foreach (var key in _Accounts.Keys)
        //                {
        //                    if (i == count)
        //                        break;
        //                    var o = _Accounts[key];
        //                    if (o.HasValue)
        //                    {
        //                        items[i++] = new KeyValuePair<ushort, Account>(key, (Account)o.Value);
        //                    }
        //                }

        //                count = i;

        //                writer.Write((ushort)count);

        //                for (i = 0; i < count; i++)
        //                {
        //                    var item = items[i];

        //                    var account = item.Value;
        //                    writer.Write(account.UID);
        //                    writer.Write(account.Name);
        //                    writer.Write(account.WindowsAccount);
        //                    writer.Write(account.CreatedUtc.ToBinary());
        //                    writer.Write(account.LastUsedUtc.ToBinary());
        //                    writer.Write(account.TotalUses);
        //                    writer.Write(account.Arguments);

        //                    booleans = new bool[]
        //                    {
        //                        account.ShowDaily,
        //                        account.Windowed,
        //                        account.RecordLaunches,
        //                        account.AutomaticLogin,
        //                        account.DatFile != null
        //                    };

        //                    b = CompressBooleans(booleans);

        //                    writer.Write((byte)b.Length);
        //                    writer.Write(b);

        //                    if (booleans[4])
        //                    {
        //                        writer.Write(account.DatFile.UID);
        //                    }

        //                    writer.Write(account.WindowBounds.X);
        //                    writer.Write(account.WindowBounds.Y);
        //                    writer.Write(account.WindowBounds.Width);
        //                    writer.Write(account.WindowBounds.Height);

        //                    if (booleans[3])
        //                    {
        //                        writer.Write(account.AutomaticLoginEmail);
        //                        writer.Write(account.AutomaticLoginPassword);
        //                    }
        //                }
        //            }

        //            lock (_HiddenUserAccounts)
        //            {
        //                var count = _HiddenUserAccounts.Count;
        //                var items = new string[count];
        //                int i = 0;

        //                foreach (var key in _HiddenUserAccounts.Keys)
        //                {
        //                    if (i == count)
        //                        break;
        //                    var o = _HiddenUserAccounts[key];
        //                    if (o.HasValue && o.Value)
        //                    {
        //                        items[i++] = key;
        //                    }
        //                }

        //                count = i;

        //                writer.Write((ushort)count);

        //                for (i = 0; i < count; i++)
        //                {
        //                    var item = items[i];
        //                    writer.Write(item);
        //                }
        //            }
        //        }

        //        var tmp = new FileInfo(path + ".tmp");
        //        if (tmp.Length > 0)
        //        {
        //            if (File.Exists(path))
        //                File.Delete(path);
        //            File.Move(path + ".tmp", path);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Util.Logging.Log(e);
        //        return e;
        //    }

        //    return null;
        //}

        #endregion

        private static bool Compare(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;

            for (int i = a.Length - 1; i >= 0; i--)
            {
                if (a[i] != b[i])
                    return false;
            }

            return true;
        }

        private static Type GetWindow(byte id)
        {
            if (id < FORMS.Length)
                return FORMS[id];

            return null;
        }

        private static byte GetWindowID(Type t)
        {
            for (byte i = 0; i < FORMS.Length; i++)
            {
                if (t == FORMS[i])
                    return i;
            }

            return byte.MaxValue;
        }

        private static SettingValue<Font> _FontName;
        public static ISettingValue<Font> FontName
        {
            get
            {
                return _FontName;
            }
        }

        private static SettingValue<Font> _FontStatus;
        public static ISettingValue<Font> FontStatus
        {
            get
            {
                return _FontStatus;
            }
        }

        private static SettingValue<Font> _FontUser;
        public static ISettingValue<Font> FontUser
        {
            get
            {
                return _FontUser;
            }
        }

        private static SettingValue<bool> _StyleShowAccount;
        public static ISettingValue<bool> StyleShowAccount
        {
            get
            {
                return _StyleShowAccount;
            }
        }

        private static SettingValue<bool> _MinimizeToTray;
        public static ISettingValue<bool> MinimizeToTray
        {
            get
            {
                return _MinimizeToTray;
            }
        }

        private static SettingValue<bool> _ShowTray;
        public static ISettingValue<bool> ShowTray
        {
            get
            {
                return _ShowTray;
            }
        }

        private static SettingValue<bool> _TopMost;
        public static ISettingValue<bool> TopMost
        {
            get
            {
                return _TopMost;
            }
        }

        private static SettingValue<bool> _BringToFrontOnExit;
        public static ISettingValue<bool> BringToFrontOnExit
        {
            get
            {
                return _BringToFrontOnExit;
            }
        }

        private static KeyedProperty<Type, Rectangle> _WindowBounds;
        public static IKeyedProperty<Type, Rectangle> WindowBounds
        {
            get
            {
                return _WindowBounds;
            }
        }

        private static KeyedProperty<string, bool> _HiddenUserAccounts;
        public static IKeyedProperty<string, bool> HiddenUserAccounts
        {
            get
            {
                return _HiddenUserAccounts;
            }
        }

        private static KeyedProperty<ushort, IAccount> _Accounts;
        public static IKeyedProperty<ushort, IAccount> Accounts
        {
            get
            {
                return _Accounts;
            }
        }

        private static KeyedProperty<ushort, IDatFile> _DatFiles;
        public static IKeyedProperty<ushort, IDatFile> DatFiles
        {
            get
            {
                return _DatFiles;
            }
        }

        private static KeyedProperty<ushort, IGfxFile> _GfxFiles;
        public static IKeyedProperty<ushort, IGfxFile> GfxFiles
        {
            get
            {
                return _GfxFiles;
            }
        }

        private static KeyedProperty<ushort, IGwDatFile> _GwDatFiles;
        public static IKeyedProperty<ushort, IGwDatFile> GwDatFiles
        {
            get
            {
                return _GwDatFiles;
            }
        }

        private static SettingValue<bool> _StoreCredentials;
        public static ISettingValue<bool> StoreCredentials
        {
            get
            {
                return _StoreCredentials;
            }
        }

        private static SettingValue<bool> _DeleteCacheOnLaunch;
        public static ISettingValue<bool> DeleteCacheOnLaunch
        {
            get
            {
                return _DeleteCacheOnLaunch;
            }
        }


        private static SettingValue<bool> _CheckForNewBuilds;
        public static ISettingValue<bool> CheckForNewBuilds
        {
            get
            {
                return _CheckForNewBuilds;
            }
        }

        private static SettingValue<int> _LastKnownBuild;
        public static ISettingValue<int> LastKnownBuild
        {
            get
            {
                return _LastKnownBuild;
            }
        }

        private static SettingValue<LastCheckedVersion> _LastProgramVersion;
        public static ISettingValue<LastCheckedVersion> LastProgramVersion
        {
            get
            {
                return _LastProgramVersion;
            }
        }

        private static SettingValue<ushort> _AutoUpdateInterval;
        public static ISettingValue<ushort> AutoUpdateInterval
        {
            get
            {
                return _AutoUpdateInterval;
            }
        }

        private static SettingValue<bool> _AutoUpdate;
        public static ISettingValue<bool> AutoUpdate
        {
            get
            {
                return _AutoUpdate;
            }
        }

        private static SettingValue<bool> _BackgroundPatchingEnabled;
        public static ISettingValue<bool> BackgroundPatchingEnabled
        {
            get
            {
                return _BackgroundPatchingEnabled;
            }
        }

        private static SettingValue<ScreenAttachment> _BackgroundPatchingNotifications;
        public static ISettingValue<ScreenAttachment> BackgroundPatchingNotifications
        {
            get
            {
                return _BackgroundPatchingNotifications;
            }
        }

        private static SettingValue<Rectangle> _BackgroundPatchingProgress;
        public static ISettingValue<Rectangle> BackgroundPatchingProgress
        {
            get
            {
                return _BackgroundPatchingProgress;
            }
        }

        private static SettingValue<byte> _BackgroundPatchingLang;
        public static ISettingValue<byte> BackgroundPatchingLang
        {
            get
            {
                return _BackgroundPatchingLang;
            }
        }

        private static SettingValue<byte> _BackgroundPatchingMaximumThreads;
        public static ISettingValue<byte> BackgroundPatchingMaximumThreads
        {
            get
            {
                return _BackgroundPatchingMaximumThreads;
            }
        }

        private static SettingValue<bool> _LocalAssetServerEnabled;
        public static ISettingValue<bool> LocalAssetServerEnabled
        {
            get
            {
                return _LocalAssetServerEnabled;
            }
        }

        private static SettingValue<PatchingFlags> _PatchingOptions;
        public static ISettingValue<PatchingFlags> PatchingOptions
        {
            get
            {
                return _PatchingOptions;
            }
        }

        private static SettingValue<int> _PatchingSpeedLimit;
        public static ISettingValue<int> PatchingSpeedLimit
        {
            get
            {
                return _PatchingSpeedLimit;
            }
        }

        private static SettingValue<bool> _PreventTaskbarGrouping;
        public static ISettingValue<bool> PreventTaskbarGrouping
        {
            get
            {
                return _PreventTaskbarGrouping;
            }
        }

        private static SettingValue<string> _WindowCaption;
        public static ISettingValue<string> WindowCaption
        {
            get
            {
                return _WindowCaption;
            }
        }

        private static SettingValue<ButtonAction> _ActionInactiveLClick;
        public static ISettingValue<ButtonAction> ActionInactiveLClick
        {
            get
            {
                return _ActionInactiveLClick;
            }
        }

        private static SettingValue<ButtonAction> _ActionActiveLClick;
        public static ISettingValue<ButtonAction> ActionActiveLClick
        {
            get
            {
                return _ActionActiveLClick;
            }
        }

        private static SettingValue<ButtonAction> _ActionInactiveMClick;
        public static ISettingValue<ButtonAction> ActionInactiveMClick
        {
            get
            {
                return _ActionInactiveMClick;
            }
        }

        private static SettingValue<ButtonAction> _ActionActiveMClick;
        public static ISettingValue<ButtonAction> ActionActiveMClick
        {
            get
            {
                return _ActionActiveMClick;
            }
        }

        private static SettingValue<ButtonAction> _ActionActiveLPress;
        public static ISettingValue<ButtonAction> ActionActiveLPress
        {
            get
            {
                return _ActionActiveLPress;
            }
        }

        private static SettingValue<DailiesMode> _ShowDailies;
        public static ISettingValue<DailiesMode> ShowDailies
        {
            get
            {
                return _ShowDailies;
            }
        }

        private static KeyedProperty<byte, bool> _HiddenDailyCategories;
        public static IKeyedProperty<byte, bool> HiddenDailyCategories
        {
            get
            {
                return _HiddenDailyCategories;
            }
        }

        private static SettingValue<NetworkAuthorizationFlags> _NetworkAuthorization;
        public static ISettingValue<NetworkAuthorizationFlags> NetworkAuthorization
        {
            get
            {
                return _NetworkAuthorization;
            }
        }

        private static SettingValue<string> _ScreenshotNaming;
        public static ISettingValue<string> ScreenshotNaming
        {
            get
            {
                return _ScreenshotNaming;
            }
        }

        private static SettingValue<ScreenshotConversionOptions> _ScreenshotConversion;
        public static ISettingValue<ScreenshotConversionOptions> ScreenshotConversion
        {
            get
            {
                return _ScreenshotConversion;
            }
        }

        private static SettingValue<bool> _DeleteCrashLogsOnLaunch;
        public static ISettingValue<bool> DeleteCrashLogsOnLaunch
        {
            get
            {
                return _DeleteCrashLogsOnLaunch;
            }
        }

        private static SettingValue<NotificationScreenAttachment> _NotesNotifications;
        public static ISettingValue<NotificationScreenAttachment> NotesNotifications
        {
            get
            {
                return _NotesNotifications;
            }
        }

        private static SettingValue<byte> _MaxPatchConnections;
        public static ISettingValue<byte> MaxPatchConnections
        {
            get
            {
                return _MaxPatchConnections;
            }
        }

        private static SettingValue<bool> _StyleShowColor;
        public static ISettingValue<bool> StyleShowColor
        {
            get
            {
                return _StyleShowColor;
            }
        }

        private static SettingValue<bool> _StyleHighlightFocused;
        public static ISettingValue<bool> StyleHighlightFocused
        {
            get
            {
                return _StyleHighlightFocused;
            }
        }

        private static SettingValue<bool> _WindowIcon;
        public static ISettingValue<bool> WindowIcon
        {
            get
            {
                return _WindowIcon;
            }
        }

        private static SettingValue<bool> _UseDefaultIconForShortcuts;
        public static ISettingValue<bool> UseDefaultIconForShortcuts
        {
            get
            {
                return _UseDefaultIconForShortcuts;
            }
        }

        private static SettingValue<bool> _AccountBarEnabled;
        private static SettingValue<AccountBarOptions> _AccountBarOptions;
        private static SettingValue<AccountBarStyles> _AccountBarStyle;
        private static SettingValue<SortingOptions> _AccountBarSorting;
        private static SettingValue<ScreenAnchor> _AccountBarDocked;

        public static class AccountBar
        {
            public static ISettingValue<bool> Enabled
            {
                get
                {
                    return _AccountBarEnabled;
                }
            }

            public static ISettingValue<AccountBarOptions> Options
            {
                get
                {
                    return _AccountBarOptions;
                }
            }

            public static ISettingValue<AccountBarStyles> Style
            {
                get
                {
                    return _AccountBarStyle;
                }
            }

            public static ISettingValue<SortingOptions> Sorting
            {
                get
                {
                    return _AccountBarSorting;
                }
            }

            public static ISettingValue<ScreenAnchor> Docked
            {
                get
                {
                    return _AccountBarDocked;
                }
            }
        }

        private static SettingValue<byte> _LimitActiveAccounts;
        public static ISettingValue<byte> LimitActiveAccounts
        {
            get
            {
                return _LimitActiveAccounts;
            }
        }

        private static SettingValue<bool> _DelayLaunchUntilLoaded;
        public static ISettingValue<bool> DelayLaunchUntilLoaded
        {
            get
            {
                return _DelayLaunchUntilLoaded;
            }
        }

        private static SettingValue<byte> _DelayLaunchSeconds;
        public static ISettingValue<byte> DelayLaunchSeconds
        {
            get
            {
                return _DelayLaunchSeconds;
            }
        }

        private static SettingValue<bool> _ShowKillAllAccounts;
        public static ISettingValue<bool> ShowKillAllAccounts
        {
            get
            {
                return _ShowKillAllAccounts;
            }
        }

        private static SettingValue<bool> _RepaintInitialWindow;
        public static ISettingValue<bool> RepaintInitialWindow
        {
            get
            {
                return _RepaintInitialWindow;
            }
        }

        private static Gw1Settings _GuildWars1;
        public static IGw1Settings GuildWars1
        {
            get
            {
                return _GuildWars1;
            }
        }

        private static Gw2Settings _GuildWars2;
        public static IGw2Settings GuildWars2
        {
            get
            {
                return _GuildWars2;
            }
        }

        private static SettingValue<EncryptionOptions> _Encryption;
        public static ISettingValue<EncryptionOptions> Encryption
        {
            get
            {
                return _Encryption;
            }
        }

        private static SettingValue<ScreenAttachment> _ScreenshotNotifications;
        public static ISettingValue<ScreenAttachment> ScreenshotNotifications
        {
            get
            {
                return _ScreenshotNotifications;
            }
        }

        private static SettingValue<ushort> _PatchingPort;
        public static ISettingValue<ushort> PatchingPort
        {
            get
            {
                return _PatchingPort;
            }
        }

        private static SettingValue<bool> _StyleShowIcon;
        public static ISettingValue<bool> StyleShowIcon
        {
            get
            {
                return _StyleShowIcon;
            }
        }

        private static SettingValue<byte> _StyleColumns;
        public static ISettingValue<byte> StyleColumns
        {
            get
            {
                return _StyleColumns;
            }
        }

        private static SettingValue<SortingOptions> _Sorting;
        public static ISettingValue<SortingOptions> Sorting
        {
            get
            {
                return _Sorting;
            }
        }

        private static SettingValue<string> _StyleBackgroundImage;
        public static ISettingValue<string> StyleBackgroundImage
        {
            get
            {
                return _StyleBackgroundImage;
            }
        }

        private static SettingValue<AccountGridButtonColors> _StyleColors;
        public static ISettingValue<AccountGridButtonColors> StyleColors
        {
            get
            {
                return _StyleColors;
            }
        }

        private static ListProperty<WindowTemplate> _WindowTemplates;
        public static IListProperty<WindowTemplate> WindowTemplates
        {
            get
            {
                return _WindowTemplates;
            }
        }

        private static SettingValue<bool> _LaunchBehindOtherAccounts;
        public static ISettingValue<bool> LaunchBehindOtherAccounts
        {
            get
            {
                return _LaunchBehindOtherAccounts;
            }
        }

        private static SettingValue<LaunchLimiterOptions> _LaunchLimiter;
        public static ISettingValue<LaunchLimiterOptions> LaunchLimiter
        {
            get
            {
                return _LaunchLimiter;
            }
        }

        private static SettingValue<bool> _ShowLaunchAllAccounts;
        public static ISettingValue<bool> ShowLaunchAllAccounts
        {
            get
            {
                return _ShowLaunchAllAccounts;
            }
        }

        private static SettingValue<byte> _LaunchTimeout;
        public static ISettingValue<byte> LaunchTimeout
        {
            get
            {
                return _LaunchTimeout;
            }
        }

        private static SettingValue<byte> _SelectedPage;
        public static ISettingValue<byte> SelectedPage
        {
            get
            {
                return _SelectedPage;
            }
        }

        private static SettingValue<JumpListOptions> _JumpList;
        public static ISettingValue<JumpListOptions> JumpList
        {
            get
            {
                return _JumpList;
            }
        }

        private static SettingValue<bool> _PreventTaskbarMinimize;
        public static ISettingValue<bool> PreventTaskbarMinimize
        {
            get
            {
                return _PreventTaskbarMinimize;
            }
        }

        private static SettingValue<bool> _AuthenticatorPastingEnabled;
        public static ISettingValue<bool> AuthenticatorPastingEnabled
        {
            get
            {
                return _AuthenticatorPastingEnabled;
            }
        }

        private static SettingValue<Settings.ProcessPriorityClass> _ProcessPriority;
        public static ISettingValue<Settings.ProcessPriorityClass> ProcessPriority
        {
            get
            {
                return _ProcessPriority;
            }
        }

        private static SettingValue<byte[]> _PublicIPAddress;
        public static ISettingValue<byte[]> PublicIPAddress
        {
            get
            {
                return _PublicIPAddress;
            }
        }

        public static bool DisableAutomaticLogins
        {
            get;
            set;
        }

        public static bool Silent
        {
            get;
            set;
        }

        public static bool ReadOnly
        {
            get;
            set;
        }

        public static bool IsRunningWine
        {
            get;
            set;
        }

        public static ushort GetNextUID()
        {
            return (ushort)(_accountUID + 1);
        }

        private static ushort GetNextSortKey()
        {
            int key = 0;

            lock (_Accounts)
            {
                foreach (var uid in _Accounts.Keys)
                {
                    var a = (Account)_Accounts[uid].Value;
                    if (a != null && a._SortKey > key && a._SortKey != ushort.MaxValue)
                        key = a._SortKey;
                }
            }

            return (ushort)(key + 1);
        }

        public static IAccount CreateAccount(Settings.AccountType type)
        {
            lock (_Accounts)
            {
                Account account;

                switch (type)
                {
                    case AccountType.GuildWars1:

                        account = new Gw1Account(++_accountUID);

                        break;
                    case AccountType.GuildWars2:

                        account = new Gw2Account(++_accountUID);

                        break;
                    default:

                        throw new NotSupportedException();
                }

                account._Type = type;
                account._SortKey = GetNextSortKey();
                account._LastUsedUtc = DateTime.MinValue;
                account._CreatedUtc = DateTime.UtcNow;
                
                _Accounts.Add(account.UID, new SettingValue<IAccount>(account));
                return account;
            }
        }

        public static IAccount Clone(IAccount account)
        {
            var a = (Account)account;
            var b = a.Clone();

            lock (_Accounts)
            {
                b._UID = ++_accountUID;
                b._SortKey = GetNextSortKey();
                _Accounts.Add(b.UID, new SettingValue<IAccount>(b));

                return b;
            }
        }

        public static IAccount CreateVoidAccount(Settings.AccountType type)
        {
            switch (type)
            {
                case AccountType.GuildWars1:

                    return new Account(0);

                default:

                    return new Account(0);
            }
        }

        public static IDatFile CreateVoidDatFile()
        {
            return new DatFile(0);
        }

        public static IDatFile CreateDatFile()
        {
            lock (_DatFiles)
            {
                var dat = new DatFile(++_datUID);
                _DatFiles.Add(dat.UID, new SettingValue<IDatFile>(dat));
                return dat;
            }
        }

        public static void RemoveDatFile(IDatFile file)
        {
            lock (_DatFiles)
            {
                if (_datUID == file.UID)
                    _datUID--;
                _DatFiles.Remove(file.UID);
            }
        }

        public static IGfxFile CreateGfxFile()
        {
            lock (_GfxFiles)
            {
                var gfx = new GfxFile(++_gfxUID);
                _GfxFiles.Add(gfx.UID, new SettingValue<IGfxFile>(gfx));
                return gfx;
            }
        }

        public static void RemoveGfxFile(IGfxFile file)
        {
            lock (_GfxFiles)
            {
                if (_gfxUID == file.UID)
                    _gfxUID--;
                _GfxFiles.Remove(file.UID);
            }
        }

        public static IGwDatFile CreateGwDatFile()
        {
            lock (_GwDatFiles)
            {
                var dat = new GwDatFile(++_gwdatUID);
                _GwDatFiles.Add(dat.UID, new SettingValue<IGwDatFile>(dat));
                return dat;
            }
        }

        public static void RemoveGwDatFile(IGwDatFile file)
        {
            lock (_GwDatFiles)
            {
                if (_gwdatUID == file.UID)
                    _gwdatUID--;
                _GwDatFiles.Remove(file.UID);
            }
        }

        public static IAccountTypeSettings GetSettings(AccountType type)
        {
            switch (type)
            {
                case AccountType.GuildWars1:

                    return _GuildWars1;

                case AccountType.GuildWars2:

                    return _GuildWars2;
            }

            return null;
        }

        public static void Save()
        {
            Task t;
            lock (_lock)
            {
                if (task == null)
                    return;
                t = task;
                cancelWrite.Cancel();
            }
            t.Wait();
        }
    }
}
