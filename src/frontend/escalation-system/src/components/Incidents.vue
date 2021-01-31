<template>
  <div class="ma-5">
    <h2>Incidentes</h2>

    <div v-for="incident in incidents" :key="incident.id" style="border-bottom: solid 1px rgb(193, 199, 206)"
         class="ma-5">
      <div
          class="d-flex flex-row lane"
          flat
          tile
      >

        <div class="incident">
          <v-btn elevation="2" class="btn-incident" @click="showIncidentDetail(incident)">
            incidente <br>
            {{ incident.taylorId }}
          </v-btn>
          <v-dialog v-model="dialog" width="500">


            <v-card>
              <v-card-title class="pl-4">
                Detalhes do incidente
              </v-card-title>

              <v-list-item>
                <v-list-item-content>
                  <v-list-item-title class="font-weight-bold">TaylorId</v-list-item-title>
                  <v-list-item-subtitle>{{ incidentDetail.taylorId}}</v-list-item-subtitle>
                </v-list-item-content>
              </v-list-item>

              <v-list-item>
                <v-list-item-content>
                  <v-list-item-title class="font-weight-bold">Descrição</v-list-item-title>
                  <v-list-item-subtitle>{{ incidentDetail.description}}</v-list-item-subtitle>
                </v-list-item-content>
              </v-list-item>

              <v-list-item>
                <v-list-item-content>
                  <v-list-item-title class="font-weight-bold">Data da abertura</v-list-item-title>
                  <v-list-item-subtitle>{{ formatDateTime(incidentDetail.dateTime)}}</v-list-item-subtitle>
                </v-list-item-content>
              </v-list-item>

              <v-list-item>
                <v-list-item-content>
                  <v-list-item-title class="font-weight-bold">Atendente</v-list-item-title>
                  <v-list-item-subtitle>{{ incidentDetail.incidentOwner.name}}</v-list-item-subtitle>
                </v-list-item-content>
              </v-list-item>

              <v-list-item>
                <v-list-item-content>
                  <v-list-item-title class="font-weight-bold">Celular do atendente</v-list-item-title>
                  <v-list-item-subtitle>{{ incidentDetail.incidentOwner.cellphone}}</v-list-item-subtitle>
                </v-list-item-content>
              </v-list-item>

              <v-list-item>
                <v-list-item-content>
                  <v-list-item-title class="font-weight-bold">Email do atendente</v-list-item-title>
                  <v-list-item-subtitle>{{ incidentDetail.incidentOwner.email}}</v-list-item-subtitle>
                </v-list-item-content>
              </v-list-item>

              <v-divider></v-divider>

              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn
                    color="primary"
                    text
                    @click="dialog = false"
                >
                  Fechar
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </div>

        <div class="next">
          <v-icon large>
            mdi-arrow-right-thick
          </v-icon>
        </div>

        <div v-for="event in incident.events" :key="event.id" class="d-flex flex-row event">
          <v-tooltip bottom>
            <template v-slot:activator="{ on, attrs }">
              <div elevation="2"
                   :class="'d-flex flex-column justify-space-between align-center elevation-2 btn-incident ' + (event.status === 2 ? 'lost' : event.status === 1 || event.status === 3 ? 'answered' : 'calling')"
                   v-bind="attrs"
                   v-on="on"
              >
              <span class="text-center pt-1">
                  {{ event.employee }}
              </span>

                <div style="padding-bottom: 5px">
                  <v-icon large v-if="event.status === 0">
                    mdi-cellphone-sound
                  </v-icon>
                  <v-icon large v-if="event.status === 1">
                    mdi-cellphone
                  </v-icon>
                  <v-icon large v-if="event.status === 2">
                    mdi-cellphone-off
                  </v-icon>
                  <v-icon large v-if="event.status === 3">
                    mdi-email-send
                  </v-icon>
                </div>
              </div>
            </template>
            <span v-if="event.status === 0">Estamos ligando nesse momento {{ formatDateTime(event.dateTime) }}</span>
            <span v-if="event.status === 1">Atendeu a ligação em {{ formatDateTime(event.dateTime) }}</span>
            <span v-if="event.status === 2">Não atendeu quando ligamos em {{ formatDateTime(event.dateTime) }}</span>
            <span v-if="event.status === 3">Como ninguém respondeu as ligações enviamos um e-mail ao dono do chamado em {{ formatDateTime(event.dateTime) }}</span>
          </v-tooltip>

          <div class="next">
            <v-icon large>
              mdi-arrow-right-thick
            </v-icon>
          </div>
        </div>

      </div>

      <div class="ma-3 ml-0 grey--text text-sm-body-2 ">
        última atualização em: {{ new Date().toLocaleDateString()}} {{ new Date().toLocaleTimeString()}}
      </div>
    </div>
    {{ this.info }}
  </div>
</template>

<script>
export default {
  name: "Incidents",
  data: () => ({
    info: null,
    interval: undefined,
    intervalInSeconds: 30 * 1000,
    incidentDetail: {
      id: '',
      taylorId: '',
      description: '',
      dateTime: '',
      teamId: '',
      incidentOwner: {
        id: '',
        sapId: '',
        name: '',
        cellphone: '',
        email: ''
      }
    },
    dialog: false,
    incidents: []
  }),
  mounted() {
    this.updateIncidents();
    this.interval = setInterval(this.updateIncidents
     , this.intervalInSeconds);
  },
  methods: {
    formatDateTime(datetime) {
      let date = new Date(datetime).toLocaleDateString()
      let time = new Date(datetime).toLocaleTimeString()

      return date + " " + time;
    },
    showIncidentDetail(incident) {
      this.dialog = true;
      this.incidentDetail = incident;
    },
    updateIncidents() {
      if(!this.dialog) {
        this.$http
            .get('http://localhost:7071/api/ListAllIncidentsFake')
            .then(response => (this.incidents = response.data));
      }
    }
  }
}
</script>

<style scoped>
.lane, .event {
  height: 100px;
  align-items: center;
}

.btn-incident {
  height: 100px !important;;
  width: 100px !important;
  border-radius: 4px;
  justify-content: center;
  overflow: hidden;
}

.answered {
  background: #D5E8D4 !important;
  border: solid 1px #82B366;
  color: #82B366;
}

.answered .v-icon {
  color: #82B366;
}

.lost {
  background: #F8CECC !important;
  border: solid 1px #B85450;
  color: #d0c224 !important;
}

.lost .v-icon {
  color: #B85450 !important;
}

.calling {
  background: #DAE8FC !important;
  border: solid 1px #6C8EBF;
  color: #6C8EBF;
}

.calling .v-icon {
  color: #6C8EBF;
}

.next {
  padding-left: 10px;
  padding-right: 10px;
}

.event:last-child .next {
  display: none;
}
</style>