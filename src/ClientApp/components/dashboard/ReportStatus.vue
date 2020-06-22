<script lang="ts">
import { Vue, Component } from 'nuxt-property-decorator'
import { Pie } from 'vue-chartjs'

@Component({
  extends: Pie
})
export default class ReportStatus extends Vue {
  data = {
    labels: ['Pending', 'Completed', 'Closed', 'Green', 'Purple', 'Orange'],
    datasets: [{
      label: '# of Reports',
      data: [12, 19, 3, 5, 2, 3],
      backgroundColor: [
        'rgba(255, 99, 132, 0.2)',
        'rgba(54, 162, 235, 0.2)',
        'rgba(255, 206, 86, 0.2)',
        'rgba(75, 192, 192, 0.2)',
        'rgba(153, 102, 255, 0.2)',
        'rgba(255, 159, 64, 0.2)'
      ],
      borderColor: [
        'rgba(255, 99, 132, 1)',
        'rgba(54, 162, 235, 1)',
        'rgba(255, 206, 86, 1)',
        'rgba(75, 192, 192, 1)',
        'rgba(153, 102, 255, 1)',
        'rgba(255, 159, 64, 1)'
      ],
      borderWidth: 1
    }]
  }

  options = {
    scales: {
      yAxes: [{
        ticks: {
          beginAtZero: true
        }
      }]
    }
  }

  mounted () {
    this.renderChart(this.data, this.options)
  }
}
</script>
