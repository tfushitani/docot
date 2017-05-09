package jp.or.hills_kobayashi.docot.model;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.Id;
import java.sql.Timestamp;

@Entity
public class DeviceHistory {

    @Id
    @GeneratedValue
    private Long historySeq;

    @Column
    private String deviceId;

    @Column
    private Timestamp firstDate;

    @Column
    private Timestamp lastDate;

    @Column
    private String cityCode;

    @Column
    private String cityName;

    public Long getHistorySeq() {
        return historySeq;
    }

    public void setHistorySeq(Long historySeq) {
        this.historySeq = historySeq;
    }

    public String getDeviceId() {
        return deviceId;
    }

    public void setDeviceId(String deviceId) {
        this.deviceId = deviceId;
    }

    public Timestamp getFirstDate() {
        return firstDate;
    }

    public void setFirstDate(Timestamp firstDate) {
        this.firstDate = firstDate;
    }

    public Timestamp getLastDate() {
        return lastDate;
    }

    public void setLastDate(Timestamp lastDate) {
        this.lastDate = lastDate;
    }

    public String getCityCode() {
        return cityCode;
    }

    public void setCityCode(String cityCode) {
        this.cityCode = cityCode;
    }

    public String getCityName() {
        return cityName;
    }

    public void setCityName(String cityName) {
        this.cityName = cityName;
    }
}
