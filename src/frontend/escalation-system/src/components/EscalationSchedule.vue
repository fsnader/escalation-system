<template>
  <div class="ma-5">
    <v-sheet
        tile
        height="54"
        class="d-flex"
    >
      <v-btn
          icon
          class="ma-2"
          @click="$refs.calendar.prev()"
      >
        <v-icon>mdi-chevron-left</v-icon>
      </v-btn>
      <v-select
          v-model="selectedTime"
          :items="times"
          dense
          outlined
          hide-details
          class="ma-2"
      ></v-select>
      <v-spacer></v-spacer>
      <v-btn
          icon
          class="ma-2"
          @click="$refs.calendar.next()"
      >
        <v-icon>mdi-chevron-right</v-icon>
      </v-btn>
    </v-sheet>
    <v-sheet height="600">
      <v-calendar
          ref="calendar"
          v-model="value"
          :weekdays="weekday"
          :type="type"
          :events="events"
          :event-overlap-mode="mode"
          :event-overlap-threshold="30"
          :event-color="getEventColor"
          @change="getEvents"
      ></v-calendar>
    </v-sheet>
  </div>
</template>

<script>
export default {
  name: "EscalationSchedule",
  data: () => ({
    selectedTime: 'Devs PIX',
    times: ['Devs PIX', 'Estagiários'],
    type: 'month',
    types: ['month', 'week', 'day', '4day'],
    mode: 'stack',
    modes: ['stack', 'column'],
    weekday: [0, 1, 2, 3, 4, 5, 6],
    weekdays: [
      { text: 'Sun - Sat', value: [0, 1, 2, 3, 4, 5, 6] },
      { text: 'Mon - Sun', value: [1, 2, 3, 4, 5, 6, 0] },
      { text: 'Mon - Fri', value: [1, 2, 3, 4, 5] },
      { text: 'Mon, Wed, Fri', value: [1, 3, 5] },
    ],
    value: '',
    events: [],
    colors: ['blue', 'green', 'orange', 'grey darken-1'],
    names: ['Alberto dev1', 'Bruno dev2','Caio dev3', 'Denis dev4'],
  }),
  methods: {
    getEvents ({ start, end }) {
      const events = []

      for (var d = new Date(start.date); d <= new Date(end.date); d.setDate(d.getDate() + 1)) {

        let pos = this.rnd(0, this.names.length - 1);
        let pos2 = pos == 3 ? 0 : pos + 1;

        events.push({
          name: this.names[pos],
          start: new Date(d),
          end: new Date(d),
          color: this.colors[pos],
          timed: true,
        })

        events.push({
          name: this.names[pos2],
          start: new Date(d),
          end: new Date(d),
          color: this.colors[pos2],
          timed: true,
        })
      }



      // console.log('start', start.date)
      // console.log('end', end.date)
      // const min = new Date(`${start.date}T00:00:00`)
      // const max = new Date(`${end.date}T23:59:59`)
      // const days = (max.getTime() - min.getTime()) / 86400000
      // const eventCount = days
      //
      // for (let i = 0; i < eventCount; i++) {
      //   const allDay = true
      //   const firstTimestamp = this.rnd(min.getTime(), max.getTime())
      //   const first = new Date(firstTimestamp - (firstTimestamp % 900000))
      //   console.log(first)
      //   const secondTimestamp = this.rnd(2, allDay ? 288 : 8) * 900000
      //   const second = new Date(first.getTime() + secondTimestamp)
      //
      //   events.push({
      //     name: this.names[this.rnd(0, this.names.length - 1)],
      //     start: first,
      //     end: second,
      //     color: this.colors[this.rnd(0, this.colors.length - 1)],
      //     timed: true,
      //   })
      // }

      this.events = events
    },
    getEventColor (event) {
      return event.color
    },
    rnd (a, b) {
      return Math.floor((b - a + 1) * Math.random()) + a
    },
  },
}
</script>
