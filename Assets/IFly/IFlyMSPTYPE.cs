using UnityEngine;
using System.Collections;

namespace Wangz
{
    /*
     *  MSPSampleStatus indicates how the sample buffer should be handled
     *  MSP_AUDIO_SAMPLE_FIRST		- The sample buffer is the start of audio
     *								  If recognizer was already recognizing, it will discard
     *								  audio received to date and re-start the recognition
     *  MSP_AUDIO_SAMPLE_CONTINUE	- The sample buffer is continuing audio
     *  MSP_AUDIO_SAMPLE_LAST		- The sample buffer is the end of audio
     *								  The recognizer will cease processing audio and
     *								  return results
     *  Note that sample statii can be combined; for example, for file-based input
     *  the entire file can be written with SAMPLE_FIRST | SAMPLE_LAST as the
     *  status.
     *  Other flags may be added in future to indicate other special audio
     *  conditions such as the presence of AGC
     */
    public enum AudioSampleState
    {
        INIT = 0x00,
        FIRST = 0x01,
        CONTINUE = 0x02,
        LAST = 0x04,
    }

    /*
     *  The enumeration MSPRecognizerStatus contains the recognition status
     *  MSP_REC_STATUS_SUCCESS				- successful recognition with partial results
     *  MSP_REC_STATUS_NO_MATCH				- recognition rejected
     *  MSP_REC_STATUS_INCOMPLETE			- recognizer needs more time to compute results
     *  MSP_REC_STATUS_NON_SPEECH_DETECTED	- discard status, no more in use
     *  MSP_REC_STATUS_SPEECH_DETECTED		- recognizer has detected audio, this is delayed status
     *  MSP_REC_STATUS_COMPLETE				- recognizer has return all result
     *  MSP_REC_STATUS_MAX_CPU_TIME			- CPU time limit exceeded
     *  MSP_REC_STATUS_MAX_SPEECH			- maximum speech length exceeded, partial results may be returned
     *  MSP_REC_STATUS_STOPPED				- recognition was stopped
     *  MSP_REC_STATUS_REJECTED				- recognizer rejected due to low confidence
     *  MSP_REC_STATUS_NO_SPEECH_FOUND		- recognizer still found no audio, this is delayed status
     */
    public enum RecognizerState
    {
        SUCCESS = 0,
        NO_MATCH = 1,
        INCOMPLETE = 2,
        NON_SPEECH_DETECTED = 3,
        SPEECH_DETECTED = 4,
        COMPLETE = 5,
        MAX_CPU_TIME = 6,
        MAX_SPEECH = 7,
        STOPPED = 8,
        REJECTED = 9,
        NO_SPEECH_FOUND = 10,
        FAILURE = NO_MATCH,
    }

    /*
     * The enumeration MSPepState contains the current endpointer state
     *  MSP_EP_LOOKING_FOR_SPEECH	- Have not yet found the beginning of speech
     *  MSP_EP_IN_SPEECH			- Have found the beginning, but not the end of speech
     *  MSP_EP_AFTER_SPEECH			- Have found the beginning and end of speech
     *  MSP_EP_TIMEOUT				- Have not found any audio till timeout
     *  MSP_EP_ERROR				- The endpointer has encountered a serious error
     *  MSP_EP_MAX_SPEECH			- Have arrive the max size of speech
     */
    public enum EndPointerState
    {
        LOOKING_FOR_SPEECH = 0,
        IN_SPEECH = 1,
        AFTER_SPEECH = 3,
        TIMEOUT = 4,
        ERROR = 5,
        MAX_SPEECH = 6,
        IDLE = 7  // internal state after stop and before start
    }
}
