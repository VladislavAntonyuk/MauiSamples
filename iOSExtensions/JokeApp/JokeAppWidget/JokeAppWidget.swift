//
//  JokeAppWidget.swift
//  JokeAppWidget
//
//  Created by Vladislav Antonyuk on 13.09.2022.
//

import WidgetKit
import SwiftUI
import Intents

struct Provider: IntentTimelineProvider {
    func placeholder(in context: Context) -> SimpleEntry {
        SimpleEntry(date: Date(), configuration: ConfigurationIntent())
    }

    func getSnapshot(for configuration: ConfigurationIntent, in context: Context, completion: @escaping (SimpleEntry) -> ()) {
        let entry = SimpleEntry(date: Date(), configuration: configuration)
        completion(entry)
    }

    func getTimeline(for configuration: ConfigurationIntent, in context: Context, completion: @escaping (Timeline<Entry>) -> ()) {
        var entries: [SimpleEntry] = []

        // Generate a timeline consisting of five entries an hour apart, starting from the current date.
        let currentDate = Date()
        for hourOffset in 0 ..< 5 {
            let entryDate = Calendar.current.date(byAdding: .hour, value: hourOffset, to: currentDate)!
            let entry = SimpleEntry(date: entryDate, configuration: configuration)
            entries.append(entry)
        }

        let timeline = Timeline(entries: entries, policy: .atEnd)
        completion(timeline)
    }
}

struct SimpleEntry: TimelineEntry {
    let date: Date
    let configuration: ConfigurationIntent
}

struct JokeAppWidgetEntryView : View {
    var entry: Provider.Entry

    @Environment(\.widgetFamily) private var family
        
        var body: some View {
            switch family {
            case .systemSmall:
                Text(entry.date, style: .time)
            case .systemMedium:
                Text(entry.date, style: .time)
            case .systemLarge, .systemExtraLarge:
                Text(entry.date, style: .time)
            case .accessoryCircular:
                Gauge(value: 10) {
                    Text(entry.date, style: .time)
                }
                .gaugeStyle(.accessoryCircularCapacity)
            case .accessoryInline:
                Text(entry.date, style: .time)
            case .accessoryRectangular:
                VStack(alignment: .leading) {
                    Text(entry.date, style: .time)
                    Text(entry.date, format: .dateTime)
                }
            default:
                EmptyView()
            }
        }
}

@main
struct JokeAppWidget: Widget {
    let kind: String = "JokeAppWidget"
    private var supportedFamilies: [WidgetFamily] {
            if #available(iOSApplicationExtension 16.0, *) {
                return [
                    .systemSmall,
                    .systemMedium,
                    .systemLarge,
                    .accessoryCircular,
                    .accessoryRectangular,
                    .accessoryInline
                ]
            } else {
                return [
                    .systemSmall,
                    .systemMedium,
                    .systemLarge
                ]
            }
        }
    
    var body: some WidgetConfiguration {
        IntentConfiguration(kind: kind, intent: ConfigurationIntent.self, provider: Provider()) { entry in
            JokeAppWidgetEntryView(entry: entry)
        }
        .configurationDisplayName("My Widget")
        .description("This is an example widget.")
        .supportedFamilies(supportedFamilies)
    }
}

struct JokeAppWidget_Previews: PreviewProvider {
    static var previews: some View {
        JokeAppWidgetEntryView(entry: SimpleEntry(date: Date(), configuration: ConfigurationIntent()))
            .previewContext(WidgetPreviewContext(family: .systemSmall))
    }
}
