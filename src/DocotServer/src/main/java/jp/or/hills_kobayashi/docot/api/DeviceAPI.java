package jp.or.hills_kobayashi.docot.api;

import jp.or.hills_kobayashi.docot.model.Device;
import jp.or.hills_kobayashi.docot.model.DeviceHistory;
import jp.or.hills_kobayashi.docot.model.DeviceHistoryEx;
import jp.or.hills_kobayashi.docot.repository.DeviceHistoryExRepository;
import jp.or.hills_kobayashi.docot.repository.DeviceHistoryRepository;
import jp.or.hills_kobayashi.docot.repository.DeviceRepository;
import jp.or.hills_kobayashi.docot.utility.ApplicationUtils;
import jp.or.hills_kobayashi.docot.utility.CommonUtils;
import jp.or.hills_kobayashi.docot.utility.ReverseGeoCoder;
import org.apache.commons.lang3.RandomStringUtils;
import org.springframework.beans.BeanUtils;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("docot/v1/devices")
public class DeviceAPI {

    @Autowired
    private DeviceRepository deviceRepository;

    @Autowired
    private DeviceHistoryExRepository deviceHistoryExRepository;

    @Autowired
    private DeviceHistoryRepository deviceHistoryRepository;

    @RequestMapping(method = RequestMethod.POST)
    @ResponseStatus(HttpStatus.CREATED)
    public Device postDevice(@RequestBody Device request) throws Exception {
        //こんにちは
        Device device = new Device();
        String deviceId = CommonUtils.createBase32UUID();
        String secret = RandomStringUtils.randomAlphanumeric(32);
        device.setDeviceId(deviceId);
        //device.setSecret(secret);
        device.setNickname(request.getNickname());

        device = deviceRepository.save(device);
        return device;
    }

    @RequestMapping(value = "{deviceId}", method = RequestMethod.PATCH)
    public Device patchDevice(@PathVariable String deviceId, @RequestBody Device request) throws Exception {

        Double latitude = request.getLatitude();
        Double longitude = request.getLongitude();

        if (latitude != null ^ longitude != null) {
            throw new RuntimeException("Bad Request");
        }

        Device device = deviceRepository.findOne(deviceId);
        if (device == null) {
            // 指定されたデバイスが存在しない場合は新たに生成する。この処理は今度消したい。
            device = new Device();
            String newDeviceId = CommonUtils.createBase32UUID();
            device.setDeviceId(newDeviceId);
        }
        BeanUtils.copyProperties(request, device, CommonUtils.getNullPropertyNames(request));
        if (latitude != null && longitude != null) {
            ReverseGeoCoder.Position position = ReverseGeoCoder.getPosition(latitude, longitude);
            device.setAddress(position.getAddress());
            device.setCityCode(position.getCityCode());
            device.setCityName(position.getCityName());
            device.setPositionUpdated(new Timestamp(System.currentTimeMillis()));

            DeviceHistoryEx deviceHistoryEx = new DeviceHistoryEx();
            BeanUtils.copyProperties(device, deviceHistoryEx);
            deviceHistoryEx.setUpdated(device.getPositionUpdated());
            deviceHistoryExRepository.save(deviceHistoryEx);
        }

        device = deviceRepository.save(device);
        return ApplicationUtils.hideSecret(device);
    }

    @RequestMapping(value = "{deviceId}", method = RequestMethod.DELETE)
    public void deleteDevice(@PathVariable String deviceId) throws Exception {
        deviceRepository.delete(deviceId);
    }

    @RequestMapping(method = RequestMethod.GET)
    public List<Device> getDevices(@RequestParam(required = false) Long positionUpdatedWithin) throws Exception {
        List<Device> devices = null;
        if (positionUpdatedWithin != null) {
            Timestamp positionUpdatedGreaterThanEqual = new Timestamp(System.currentTimeMillis() - positionUpdatedWithin);
            devices = deviceRepository.findByPositionUpdatedGreaterThanEqualOrderByPositionUpdatedDesc(positionUpdatedGreaterThanEqual);
        } else {
            devices = deviceRepository.findAll();
        }
        return ApplicationUtils.hideSecret(devices);
    }

    @RequestMapping(value = "{deviceId}", method = RequestMethod.GET)
    public Device getDevice(HttpServletResponse response, @PathVariable String deviceId) throws Exception {
        Device device = deviceRepository.findOne(deviceId);
        if (device == null) {
            response.setStatus(HttpServletResponse.SC_NOT_FOUND);
        }
        return ApplicationUtils.hideSecret(device);
    }

    /*
    @RequestMapping(value = "{deviceId}/histories",method = RequestMethod.GET)
    public List<DeviceHistory> getDeviceHistories(@PathVariable String deviceId, @RequestParam String sinceHistorySeq) throws Exception {
        List<DeviceHistory> deviceHistories = new ArrayList<DeviceHistory>();
        return deviceHistories;
    }
    */

    @RequestMapping(value = "{deviceId}/histories",method = RequestMethod.GET)
    public List<DeviceHistoryEx> getDeviceHistoriesEx(@PathVariable String deviceId, @RequestParam(required = false) Long updatedWithin) throws Exception {
        List<DeviceHistoryEx> deviceHistories;
        if (updatedWithin != null) {
            Timestamp positionUpdatedGreaterThanEqual = new Timestamp(System.currentTimeMillis() - updatedWithin);
            deviceHistories = deviceHistoryExRepository.findByDeviceIdAndUpdatedGreaterThanEqualOrderByUpdatedDesc(deviceId, positionUpdatedGreaterThanEqual);
        } else {
            deviceHistories = deviceHistoryExRepository.findByDeviceIdOrderByUpdatedDesc(deviceId);
        }
        return deviceHistories;
    }

}
