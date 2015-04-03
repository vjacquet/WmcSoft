using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Diagnostics
{
    public class TraceStream : Stream
    {
       #region Private fields

        private readonly Stream _stream;
        private readonly TraceSource _traceSource;

        #endregion

        #region Lifecycle

        private TraceStream() {
        }

        public TraceStream(Stream stream, TraceSource traceSource) {
            if (stream == null) 
                throw new ArgumentNullException("stream");
            if (traceSource == null)
                throw new ArgumentNullException("traceSource");

            _stream = stream;
            _traceSource = traceSource;
        }

        public override void Close() {
            try {
                _traceSource.TraceInformation("Closing...");
                _stream.Close();
                _traceSource.TraceInformation("Closed.");
            }
            catch (Exception exception) {
                _traceSource.TraceEvent(TraceEventType.Error, 0, "Close failed: {0}", exception.Message);
                throw;
            }
            finally {
                _traceSource.Flush();
            }
        }

        #endregion

        #region Info class Methods & Properties

        public override bool CanRead {
            get {
                try {
                    _traceSource.TraceInformation("Getting CanRead...");
                    bool value = _stream.CanRead;
                    _traceSource.TraceInformation("Got CanRead={0}.", value);
                    return value;
                }
                catch (Exception exception) {
                    _traceSource.TraceEvent(TraceEventType.Error, 0, "CanRead failed: {0}", exception.Message);
                    throw;
                }
                finally {
                    _traceSource.Flush();
                }
            }
        }

        public override bool CanSeek {
            get {
                try {
                    _traceSource.TraceInformation("Getting CanRead...");
                    bool value = _stream.CanSeek;
                    _traceSource.TraceInformation("Got CanSeek={0}.", value);
                    return value;
                }
                catch (Exception exception) {
                    _traceSource.TraceEvent(TraceEventType.Error, 0, "CanSeek failed: {0}", exception.Message);
                    throw;
                }
                finally {
                    _traceSource.Flush();
                }
            }
        }

        public override bool CanWrite {
            get {
                try {
                    _traceSource.TraceInformation("Getting CanWrite...");
                    bool value = _stream.CanWrite;
                    _traceSource.TraceInformation("Got CanWrite={0}.", value);
                    return value;
                }
                catch (Exception exception) {
                    _traceSource.TraceEvent(TraceEventType.Error, 0, "CanWrite failed: {0}", exception.Message);
                    throw;
                }
                finally {
                    _traceSource.Flush();
                }
            }
        }

        public override long Length {
            get {
                try {
                    _traceSource.TraceInformation("Getting Length...");
                    long value = _stream.Length;
                    _traceSource.TraceInformation("Got Length={0}.", value);
                    return value;
                }
                catch (Exception exception) {
                    _traceSource.TraceEvent(TraceEventType.Error, 0, "Get Length failed: {0}", exception.Message);
                    throw;
                }
                finally {
                    _traceSource.Flush();
                }
            }
        }

        public override long Position {
            get {
                try {
                    _traceSource.TraceInformation("Getting Position...");
                    long value = _stream.Position;
                    _traceSource.TraceInformation("Got Position={0}.", value);
                    return value;
                }
                catch (Exception exception) {
                    _traceSource.TraceEvent(TraceEventType.Error, 0, "Get Position failed: {0}", exception.Message);
                    throw;
                }
                finally {
                    _traceSource.Flush();
                }
            }
            set {
                try {
                    _traceSource.TraceInformation("Setting Position...");
                    _stream.Position = value;
                    _traceSource.TraceInformation("Set Position={0}.", value);
                }
                catch (Exception exception) {
                    _traceSource.TraceEvent(TraceEventType.Error, 0, "Set Position failed: {0}", exception.Message);
                    throw;
                }
                finally {
                    _traceSource.Flush();
                }
            }
        }

        public override bool CanTimeout {
            get {
                try {
                    _traceSource.TraceInformation("Getting CanTimeout...");
                    bool value = _stream.CanTimeout;
                    _traceSource.TraceInformation("Got CanTimeout={0}.", value);
                    return value;
                }
                catch (Exception exception) {
                    _traceSource.TraceEvent(TraceEventType.Error, 0, "CanTimeout failed: {0}", exception.Message);
                    throw;
                }
                finally {
                    _traceSource.Flush();
                }
            }
        }

        #endregion

        #region Read Methods & Properties

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state) {
            try {
                _traceSource.TraceInformation("Entering BeginRead with offset={0} and count={1}...", offset, count);
                IAsyncResult value = _stream.BeginRead(buffer, offset, count, callback, state);
                _traceSource.TraceInformation("Return from BeginRead.");
                return value;
            }
            catch (Exception exception) {
                _traceSource.TraceEvent(TraceEventType.Error, 0, "BeginRead failed: {0}", exception.Message);
                throw;
            }
            finally {
                _traceSource.Flush();
            }
        }

        public override int EndRead(IAsyncResult asyncResult) {
            try {
                _traceSource.TraceInformation("Entering EndRead...");
                int value = _stream.EndRead(asyncResult);
                _traceSource.TraceInformation("Return {0} from EndRead.", value);
                return value;
            }
            catch (Exception exception) {
                _traceSource.TraceEvent(TraceEventType.Error, 0, "EndRead failed: {0}", exception.Message);
                throw;
            }
            finally {
                _traceSource.Flush();
            }
        }

        public override int Read(byte[] buffer, int offset, int count) {
            try {
                _traceSource.TraceInformation("Entering Read with offset={0} and count={1}...", offset, count);
                int value = _stream.Read(buffer, offset, count);
                _traceSource.TraceInformation("Return {0} from Read.", value);
                return value;
            }
            catch (Exception exception) {
                _traceSource.TraceEvent(TraceEventType.Error, 0, "Read failed: {0}", exception.Message);
                throw;
            }
            finally {
                _traceSource.Flush();
            }
        }

        public override long Seek(long offset, SeekOrigin origin) {
            try {
                _traceSource.TraceInformation("Entering Seek with offset={0} and SeekOrigin={1}...", offset, origin.ToString());
                long value = _stream.Seek(offset, origin);
                _traceSource.TraceInformation("Return {0} from Seek.", value);
                return value;
            }
            catch (Exception exception) {
                _traceSource.TraceEvent(TraceEventType.Error, 0, "Seek failed: {0}", exception.Message);
                throw;
            }
            finally {
                _traceSource.Flush();
            }
        }

        public override int ReadTimeout {
            get {
                try {
                    _traceSource.TraceInformation("Getting ReadTimeout...");
                    int value = _stream.ReadTimeout;
                    _traceSource.TraceInformation("Got ReadTimeout={0}.", value);
                    return value;
                }
                catch (Exception exception) {
                    _traceSource.TraceEvent(TraceEventType.Error, 0, "Get ReadTimeout failed: {0}", exception.Message);
                    throw;
                }
                finally {
                    _traceSource.Flush();
                }
            }
            set {
                try {
                    _traceSource.TraceInformation("Setting ReadTimeout...");
                    _stream.ReadTimeout = value;
                    _traceSource.TraceInformation("Set ReadTimeout={0}.", value);
                }
                catch (Exception exception) {
                    _traceSource.TraceEvent(TraceEventType.Error, 0, "Set ReadTimeout failed: {0}", exception.Message);
                    throw;
                }
                finally {
                    _traceSource.Flush();
                }
            }
        }
        #endregion

        #region Write Methods & Properties

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state) {
            try {
                _traceSource.TraceInformation("Entering BeginWrite with offset={0} and count={1}...", offset, count);
                IAsyncResult value = _stream.BeginWrite(buffer, offset, count, callback, state);
                _traceSource.TraceInformation("Return from BeginWrite.");
                return value;
            }
            catch (Exception exception) {
                _traceSource.TraceEvent(TraceEventType.Error, 0, "BeginWrite failed: {0}", exception.Message);
                throw;
            }
            finally {
                _traceSource.Flush();
            }
        }

        public override void EndWrite(IAsyncResult asyncResult) {
            try {
                _traceSource.TraceInformation("Entering EndWrite...");
                _stream.EndWrite(asyncResult);
                _traceSource.TraceInformation("Return from EndWrite.");
            }
            catch (Exception exception) {
                _traceSource.TraceEvent(TraceEventType.Error, 0, "EndWrite failed: {0}", exception.Message);
                throw;
            }
            finally {
                _traceSource.Flush();
            }
        }

        public override void Write(byte[] buffer, int offset, int count) {
            try {
                _traceSource.TraceInformation("Entering Write with offset={0} and count={1}...", offset, count);
                _stream.Write(buffer, offset, count);
                _traceSource.TraceInformation("Return from Write.");
            }
            catch (Exception exception) {
                _traceSource.TraceEvent(TraceEventType.Error, 0, "Write failed: {0}", exception.Message);
                throw;
            }
            finally {
                _traceSource.Flush();
            }
        }

        public override void Flush() {
            try {
                _traceSource.TraceInformation("Flushing...");
                _stream.Flush();
                _traceSource.TraceInformation("Flushed.");
            }
            catch (Exception exception) {
                _traceSource.TraceEvent(TraceEventType.Error, 0, "Flush failed: {0}", exception.Message);
                throw;
            }
            finally {
                _traceSource.Flush();
            }
        }

        public override void SetLength(long value) {
            try {
                _traceSource.TraceInformation("Entering SetLength with value={0}...", value);
                _stream.SetLength(value);
                _traceSource.TraceInformation("Return from SetLength.");
            }
            catch (Exception exception) {
                _traceSource.TraceEvent(TraceEventType.Error, 0, "SetLength failed: {0}", exception.Message);
                throw;
            }
            finally {
                _traceSource.Flush();
            }
        }

        public override int WriteTimeout {
            get {
                try {
                    _traceSource.TraceInformation("Getting WriteTimeout...");
                    int value = _stream.WriteTimeout;
                    _traceSource.TraceInformation("Got WriteTimeout={0}.", value);
                    return value;
                }
                catch (Exception exception) {
                    _traceSource.TraceEvent(TraceEventType.Error, 0, "Get WriteTimeout failed: {0}", exception.Message);
                    throw;
                }
                finally {
                    _traceSource.Flush();
                }
            }
            set {
                try {
                    _traceSource.TraceInformation("Setting WriteTimeout...");
                    _stream.WriteTimeout = value;
                    _traceSource.TraceInformation("Set WriteTimeout={0}.", value);
                }
                catch (Exception exception) {
                    _traceSource.TraceEvent(TraceEventType.Error, 0, "Set WriteTimeout failed: {0}", exception.Message);
                    throw;
                }
                finally {
                    _traceSource.Flush();
                }
            }
        }

        #endregion
    }
}
