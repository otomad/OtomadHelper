const English = {
	translation: {
		titles: {
			home: "Home",
			source: "Source",
			score: "Score",
			audio: "Audio",
			visual: "Visual",
			track: "Track",
			track_other: "Tracks",
			sonar: "Sonar",
			shupelunker: "Shupelunker",
			shupelunker_full: "Shupelunker Tactics",
			ytp: "YTP",
			mosh: "Mosh",
			mosh_full: "Datamosh",
			tools: "Tools",
			settings: "Settings",
		},
		source: {
			trackEvent: "Track event",
			projectMedia: "Project media",
			browseFile: "Browse file",
			trim: "Trim",
			startTime: {
				_: "Start time",
				projectStart: "Project start",
				cursor: "Cursor",
			},
			belowTopAdjustmentTracks: "Below top adjustment tracks",
			removeSourceEventsAfterCompletion: "Remove source events after completion",
			selectAllEventsGenerated: "Select all events generated",
			randomOffsetForTracks: "Use random offsets for different tracks",
		},
		on: "On",
		off: "Off",
		custom: "Custom",
		enable: "Enable",
		subheader: {
			moreOptions: "More options",
			advanced: "Advanced",
			config: "Config",
			parameters: "Parameters",
		},
		score: {
			midi: "MIDI",
			ust: "UST",
			refOtherTracks: "Refer to other tracks",
			pureNotes: "Pure notes",
			encoding: "Encoding",
			bpm: {
				_: "BPM",
				dynamicMidi: "Dynamic MIDI tempo",
				midi: "MIDI tempo",
				project: "Project tempo",
			},
			timeSignature: "Time signature",
			constraint: {
				_: "Constraint note length",
				none: "Unconstrainted",
				max: "Max length",
				fixed: "Fixed length",
			},
		},
		audioVisual: {
			preview: "Preview",
			stretch: {
				_: "Stretch",
				noStretching: "No Stretching",
				flexingAndExtending: "Flexing & Extending",
				extendingOnly: "Extending Only",
				flexingOnly: "Flexing Only",
			},
			loop: "Loop",
			normalize: "Normalize",
			staticVisual: "Static",
			noLengthening: "No lengthening",
			legato: {
				_: "Legato",
				portato: "Portato",
				upToOneBeat: "Up to 1 beat",
				upToOneBar: "Up to 1 bar",
				unlimited: "Unlimited",
			},
			multitrackForChords: "Chords",
			createGroups: "Group",
			glissando: "Glissando",
			autoPan: "Pan",
			mappingVelocity: "Velocity",
			transformOfx: "TransformOFX",
			tuning: {
				_: "Tunning",
				tuningMethod: "Tuning method",
				stretchAttributes: "Stretch attributes",
				alternativeForOutOrRange: "If out of range",
				resample: "Resample",
				preserveFormant: "Preserve formant",
				basePitch: "Base pitch",
				prelisten: {
					_: "Prelisten",
					engine: "Engine",
					waveform: {
						_: "Waveform",
						sinusoid: "Sinusoid",
						triangle: "Triangle",
						square: "Square",
						sawtooth: "Sawtooth",
					},
					duration: "Duration",
					adjustAudioToBasePitch: "Adjust audio to base pitch",
				},
			},
			effects: {
				_: "Effects",
				prve: "PV Rhythm Visual Effect",
				staff: "Staff Visual Effect",
			},
			newTrack: "New track",
		},
		settings: {
			language: {
				_: "Language",
				en: "English",
				"zh-CN": "Simplified Chinese",
			},
			colorScheme: {
				_: "Color scheme",
				light: "Light",
				dark: "Dark",
				auto: "Auto",
			},
		},
		descriptions: {
			source: {
				trim: "Adjust start or end time of the specified source",
				startTime: "Specify when to start generating from the project",
			},
			score: {
				trim: "Select the generation time range of the score",
				bpm: "Specify the beats per minute",
				constraint: "Controls the note output length from the score",
				encoding: "Specify the text encoding of the file",
			},
			audioVisual: {
				stretch: "When on, the clip will be stretched instead of changing its duration",
				loop: "When the clip is lengthened to the end of the source media, playback will start over",
				normalize: "Normalize the audio, useful if the audio is quiet",
				staticVisual: "Freeze the frame at the start of the clip",
				noLengthening: {
					visual: "Freeze the frame at the end of the clip if the note is longer than it",
					audio: "Do not lengthen the clip if the note is longer than it",
				},
				legato: "Fill the gaps between notes",
				multitrackForChords: "Generates multiple tracks for chords",
				createGroups: "Create groups for video and audio clips represented by one note",
				glissando: "Creates a swirl effect if the note pitch bends or slides",
				autoPan: "Pans the audio using envelope automation",
				mappingVelocity: "Map the attack velocity of notes to the specified item",
				transformOfx: "Miscz is a pixel hard edge plugin, Enable to add keyframe properties to TransformOFX in this plugin",
				tuning: {
					stretchAttributes: "More config about the select tunning method",
					resample: "Lock stretch and pitch, adjust the stretch to change the pitch",
					preserveFormant: "Keep the voice tone while tunning",
					basePitch: "Specify what is the base pitch of the audio event",
				},
				effects: {
					prve: "Make your promotion video more rhythmic",
					staff: "Use custom patterns as notes to draw visuals similar fashion to piano staff sheets",
				},
			},
		},
	},
} as const;

export default English;
